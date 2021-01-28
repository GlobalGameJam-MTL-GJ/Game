using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private Renderer propsBoundsRenderer;
    [SerializeField] private LevelConfigSO levelConfigSO;
    private Bounds propsBounds;
    public Bounds PropsBounds => propsBounds;

    public LevelConfigSO LevelConfigSo => levelConfigSO;

    private static Dictionary<PropsMovementType, Type> MovementTypesToComponents =
        new Dictionary<PropsMovementType, Type>()
        {
            {PropsMovementType.Idle, typeof(PropsIdleMovement)},
            {PropsMovementType.RandomMovement, typeof(PropsRandomMovement)},
        };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        propsBounds = propsBoundsRenderer.bounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Type GetPropsMovementAccordingToLevelConfig()
    {
        float randomTotalChances = 0;
        foreach (var propsPossibleMovement in levelConfigSO.propsPossibleMovements)
        {
            //later on, only add the spawn chances if it respects the current min max limits for each movement at a time
            randomTotalChances += propsPossibleMovement.spawnChance;
        }

        float rand = UnityEngine.Random.Range(0, randomTotalChances);
        
        foreach (var propsPossibleMovement in levelConfigSO.propsPossibleMovements)
        {
            if (rand <= propsPossibleMovement.spawnChance)
                return MovementTypesToComponents[propsPossibleMovement.propsMovementType];
            rand -= propsPossibleMovement.spawnChance;
        }
        Debug.LogError("No movement component rolled");
        return null;
    }
}
