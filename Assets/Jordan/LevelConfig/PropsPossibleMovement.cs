using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropsPossibleMovement
{
    public PropsMovementType propsMovementType;
    [Range(0.00f, 1.00f)] public float spawnChance;
    [HideInInspector] public float adjustedSpawnChance;
    public int minimumOfThisTypeAtOnce = 0;
    public int maximumOfThisTypeAtOnce = 2;
    public int minimumOfThisTypeForTheLevel = 1;
    public int maximumOfThisTypeForTheLevel = 4;
}

[System.Serializable]
public class PropsPossibleTypes
{
    public PropsType propsType;
    [Range(0.00f, 1.00f)] public float spawnChance;
    [HideInInspector] public float adjustedSpawnChance;
    public int minimumOfThisTypeAtOnce = 0;
    public int maximumOfThisTypeAtOnce = 2;
    public int minimumOfThisTypeForTheLevel = 1;
    public int maximumOfThisTypeForTheLevel = 4;
}
