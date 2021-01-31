using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropsBuilder : MonoBehaviour
{
    public static PropsBuilder instance;
    [Range(-1,3)]
    [SerializeField] private float speedModifierPercentageSword = -0.2f;
    [Range(-1,3)]
    [SerializeField] private float speedModifierPercentageBow = -0.2f;
    [Range(-1,3)]
    [SerializeField] private float speedModifierPercentageGrimoire = -0.2f;
    [Range(-1,3)]
    [SerializeField] private float speedModifierPercentageCrown = -0.2f;
    [Range(-1,3)]
    [SerializeField] private float speedModifierPercentagePotion = -0.2f;
    private LevelConfigSO currentLevel;
    List<PropsPossibleMovement> propsPossibleMovements = new List<PropsPossibleMovement>();
    List<PropsPossibleTypes> propsPossibleTypes = new List<PropsPossibleTypes>();
    private GameObject propsPrefab;
    [SerializeField] private GameObject[] propsSpawns;
    [SerializeField] private float spawnRate = 0.7f;
    List<ActivePropsEntry> currentActiveProps = new List<ActivePropsEntry>();
    
    public bool PropsAvailable => currentActiveProps.Exists(activePropsEntry => !activePropsEntry.reserved);

    private static Dictionary<PropsMovementType, Type> MovementTypesToComponents =
        new Dictionary<PropsMovementType, Type>()
        {
            {PropsMovementType.Idle, typeof(PropsIdleMovement)},
            {PropsMovementType.RandomMovement, typeof(PropsRandomMovement)},
            {PropsMovementType.FollowPlayer, typeof(PropsFollowPlayerMovement)},
            {PropsMovementType.RunFromPlayer, typeof(PropsRunFromPlayerMovement)}
        };
    
    private Dictionary<PropsMovementType, PropsAmountTracker> MovementTypesAmountTracker = new Dictionary<PropsMovementType, PropsAmountTracker>()
    {
        {PropsMovementType.Idle, new PropsAmountTracker()},
        {PropsMovementType.RandomMovement, new PropsAmountTracker()},
        {PropsMovementType.FollowPlayer, new PropsAmountTracker()},
        {PropsMovementType.RunFromPlayer, new PropsAmountTracker()}
    };

    private Dictionary<PropsType, GameObject> PropsTypeToMeshes;
    
    private Dictionary<PropsType, PropsAmountTracker> PropsTypesAmountTracker = new Dictionary<PropsType, PropsAmountTracker>()
    {
        {PropsType.Sword, new PropsAmountTracker()},
        {PropsType.Grimoire, new PropsAmountTracker()},
        {PropsType.Bow, new PropsAmountTracker()},
        {PropsType.Crown, new PropsAmountTracker()},
        {PropsType.Potion, new PropsAmountTracker()}
    };
    
    private Dictionary<PropsType, PropsColorTracker> PropsTypesToColorTracker = new Dictionary<PropsType, PropsColorTracker>()
    {
        {PropsType.Sword, new PropsColorTracker()},
        {PropsType.Grimoire, new PropsColorTracker()},
        {PropsType.Bow, new PropsColorTracker()},
        {PropsType.Crown, new PropsColorTracker()},
        {PropsType.Potion, new PropsColorTracker()}
    };
    
    private Dictionary<PropsType, float> PropsTypesToSpeedModifiers;
    
    public Dictionary<PropsColor, Color> PropsColorsToColors = new Dictionary<PropsColor, Color>()
    {
        {PropsColor.Blue, Color.blue},
        {PropsColor.Green, Color.green},
        {PropsColor.Orange, new Color32(255, 130, 0, 255)},
        {PropsColor.Purple, Color.magenta},
    };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
        propsPrefab = Resources.Load<GameObject>("Prefabs/Props");
        PropsTypeToMeshes = new Dictionary<PropsType, GameObject>()
        {
            {PropsType.Sword, Resources.Load<GameObject>("Meshes/Sword")},
            {PropsType.Grimoire, Resources.Load<GameObject>("Meshes/Grimoire")},
            {PropsType.Bow, Resources.Load<GameObject>("Meshes/Bow")},
            {PropsType.Crown, Resources.Load<GameObject>("Meshes/Crown")},
            {PropsType.Potion, Resources.Load<GameObject>("Meshes/Potion")}
        };
        
        PropsTypesToSpeedModifiers = new Dictionary<PropsType, float>()
        {
            {PropsType.Sword, speedModifierPercentageSword},
            {PropsType.Grimoire, speedModifierPercentageGrimoire},
            {PropsType.Bow, speedModifierPercentageBow},
            {PropsType.Crown, speedModifierPercentageCrown},
            {PropsType.Potion, speedModifierPercentagePotion}
        };
    }

    private void Start()
    {
        TimeManager.instance.OnGameStart += HandleGameStart;
        CustomerOrderController.OnCustomerLeaving += UnreserveAnActiveProps;
        AssignCurrentLevelToTheBuilder(LevelManager.instance.LevelConfigSo);
    }

    private void HandleGameStart()
    {
        InvokeRepeating(nameof(BuildAProps), 1, spawnRate);
    }

    private void AssignCurrentLevelToTheBuilder(LevelConfigSO levelConfigSO)
    {
        currentLevel = levelConfigSO;
        propsPossibleMovements = currentLevel.propsPossibleMovements;
        foreach (var propsPossibleMovement in propsPossibleMovements)
        {
            propsPossibleMovement.adjustedSpawnChance = propsPossibleMovement.spawnChance;
        }
        propsPossibleTypes = currentLevel.propsPossibleTypes;
        foreach (var possibleType in propsPossibleTypes)
        {
            possibleType.adjustedSpawnChance = possibleType.spawnChance;
        }
    }
    
    public void BuildAProps()
    {
        if (currentLevel == null)
        {
            Debug.LogError("THERE IS NO LEVEL ASSIGNED TO THE PROPS BUILDER");
            return;
        }

        if (currentActiveProps.Count >= currentLevel.maximumPropsAtOnce) return;

        int randomSpawn = UnityEngine.Random.Range(0, propsSpawns.Length);
        GameObject choosenSpawn = propsSpawns[randomSpawn];
        GameObject newProps = Instantiate(propsPrefab, choosenSpawn.transform.position + new Vector3(0, 0.25f, 0), choosenSpawn.transform.rotation);
        newProps.AddComponent(GetPropsMovementAccordingToLevelConfig());
        
        ActivePropsEntry activePropsEntry = new ActivePropsEntry();
        activePropsEntry.propsType = GetPropsTypesAccordingToLevelConfig();
        GameObject mesh = Instantiate(PropsTypeToMeshes[activePropsEntry.propsType], newProps.transform);
        mesh.transform.localPosition = Vector3.zero;
        
        activePropsEntry.propsColor = GetRandomFreeColorForThisType(activePropsEntry.propsType);
        newProps.GetComponent<Props>().SetPropsTypeAndColorAndSpeedModifier(activePropsEntry.propsType, activePropsEntry.propsColor, PropsTypesToSpeedModifiers[activePropsEntry.propsType]);
        activePropsEntry.activeProps = newProps;
        currentActiveProps.Add(activePropsEntry);
        choosenSpawn.GetComponent<PropsSpawn>().SpawnTween(newProps);
    }

    private PropsColor GetRandomFreeColorForThisType(PropsType propsType)
    {
        return PropsTypesToColorTracker[propsType].GetRandomFreeColor();
    }

    private PropsType GetPropsTypesAccordingToLevelConfig()
    {
        List<PropsPossibleTypes> filteredPossibleTypes = new List<PropsPossibleTypes>();
        foreach (var possibleType in propsPossibleTypes)
        {
            if (PropsTypesAmountTracker[possibleType.propsType].totalAtThisMoment >=
                possibleType.maximumOfThisTypeAtOnce
                || PropsTypesAmountTracker[possibleType.propsType].totalSpawnedThisLevel >= possibleType.maximumOfThisTypeForTheLevel
                || !PropsTypesToColorTracker[possibleType.propsType].IsThereAnyColorAvailable())
                continue;
            
            filteredPossibleTypes.Add(possibleType);
        }
        
        float randomTotalChances = filteredPossibleTypes.Sum(filteredPossibleType => filteredPossibleType.adjustedSpawnChance);

        float rand = UnityEngine.Random.Range(0, randomTotalChances);
        
        foreach (var filteredPossibleType in filteredPossibleTypes)
        {
            if (rand <= filteredPossibleType.adjustedSpawnChance)
            {
                foreach (var possibleType in filteredPossibleTypes)
                {
                    if (possibleType == filteredPossibleType) possibleType.adjustedSpawnChance *= 0.8f;
                    else possibleType.adjustedSpawnChance *= 1.1f;
                }
                PropsType propsType = filteredPossibleType.propsType;
                PropsAmountTracker propsAmountTracker = PropsTypesAmountTracker[propsType];
                propsAmountTracker.totalAtThisMoment++;
                propsAmountTracker.totalSpawnedThisLevel++;
                return propsType;
            }
            rand -= filteredPossibleType.adjustedSpawnChance;
        }
        Debug.LogError("Potion rolled by default");
        return PropsType.Potion;
    }

    private Type GetPropsMovementAccordingToLevelConfig()
    {
        List<PropsPossibleMovement> filteredPossibleMovements = new List<PropsPossibleMovement>();
        foreach (var propsPossibleMovement in propsPossibleMovements)
        {
            if (MovementTypesAmountTracker[propsPossibleMovement.propsMovementType].totalAtThisMoment >=
                propsPossibleMovement.maximumOfThisTypeAtOnce
                || MovementTypesAmountTracker[propsPossibleMovement.propsMovementType].totalSpawnedThisLevel >= propsPossibleMovement.maximumOfThisTypeForTheLevel)
                continue;
            
            filteredPossibleMovements.Add(propsPossibleMovement);
        }

        float randomTotalChances = filteredPossibleMovements.Sum(filteredPossibleMovement => filteredPossibleMovement.adjustedSpawnChance);
        
        float rand = UnityEngine.Random.Range(0, randomTotalChances);
        
        foreach (var filteredPossibleMovement in filteredPossibleMovements)
        {
            if (rand <= filteredPossibleMovement.adjustedSpawnChance)
            {
                foreach (var possibleMovement in filteredPossibleMovements)
                {
                    if (possibleMovement == filteredPossibleMovement) possibleMovement.adjustedSpawnChance *= 0.8f;
                    else possibleMovement.adjustedSpawnChance *= 1.1f;
                }
                PropsMovementType propsMovementType = filteredPossibleMovement.propsMovementType;
                PropsAmountTracker propsAmountTracker = MovementTypesAmountTracker[propsMovementType];
                propsAmountTracker.totalAtThisMoment++;
                propsAmountTracker.totalSpawnedThisLevel++;
                return MovementTypesToComponents[propsMovementType];
            }
            rand -= filteredPossibleMovement.adjustedSpawnChance;
        }
        Debug.LogError("No movement component rolled");
        return null;
    }

    public ActivePropsEntry GetAnActiveProps()
    {
        List<ActivePropsEntry> filteredActivePropsEntries = new List<ActivePropsEntry>();
        foreach (var currentActiveProp in currentActiveProps.Where(x => x.reserved == false))
        {
            filteredActivePropsEntries.Add(currentActiveProp);
        }
        if(filteredActivePropsEntries.Count == 0) Debug.LogError("NO AVAILABLE PROPS, ALL RESERVED");
        int rand = UnityEngine.Random.Range(0, filteredActivePropsEntries.Count);
        ActivePropsEntry choosenProps = filteredActivePropsEntries[rand];
        choosenProps.reserved = true;
        return choosenProps;
    }

    public void RemoveAnActiveProps(GameObject activePropsToRemove)
    {
        foreach (var currentActiveProp in currentActiveProps)
        {
            if (currentActiveProp.activeProps != activePropsToRemove) continue;
            PropsTypesToColorTracker[currentActiveProp.propsType].MakeColorAvailable(currentActiveProp.propsColor);
            PropsTypesAmountTracker[currentActiveProp.propsType].totalAtThisMoment--;
            MovementTypesAmountTracker[currentActiveProp.activeProps.GetComponent<PropsMovement>().GetMovementType()]
                .totalAtThisMoment--;
            currentActiveProps.Remove(currentActiveProp);
            return;
        }
    }

    public void UnreserveAnActiveProps(GameObject activePropsToUnreserve)
    {
        foreach (var currentActiveProp in currentActiveProps)
        {
            if (currentActiveProp.activeProps != activePropsToUnreserve) continue;
            currentActiveProp.reserved = false;
            return;
        }
    }

    private void OnDestroy()
    {
        CustomerOrderController.OnCustomerLeaving -= UnreserveAnActiveProps;
        if(TimeManager.instance != null)
            TimeManager.instance.OnGameStart -= HandleGameStart;
    }
}
