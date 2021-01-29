using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropsBuilder : MonoBehaviour
{
    private static PropsBuilder instance;
    private LevelConfigSO currentLevel;
    List<PropsPossibleMovement> propsPossibleMovements = new List<PropsPossibleMovement>();
    private GameObject propsPrefab;
    [SerializeField] private GameObject[] propsSpawns;
    List<GameObject> currentActiveProps = new List<GameObject>();
    
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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        propsPrefab = Resources.Load<GameObject>("Prefabs/Props");
    }

    private void Start()
    {
        AssignCurrentLevelToTheBuilder(LevelManager.instance.LevelConfigSo);
        InvokeRepeating(nameof(BuildAProps), 1, 4);
    }

    private void AssignCurrentLevelToTheBuilder(LevelConfigSO levelConfigSO)
    {
        currentLevel = levelConfigSO;
        propsPossibleMovements = currentLevel.propsPossibleMovements;
        foreach (var propsPossibleMovement in propsPossibleMovements)
        {
            propsPossibleMovement.adjustedSpawnChance = propsPossibleMovement.spawnChance;
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
        currentActiveProps.Add(newProps);
        choosenSpawn.GetComponent<PropsSpawn>().SpawnTween(newProps);
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
}
