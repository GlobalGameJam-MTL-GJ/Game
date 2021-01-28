using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropsPossibleMovement
{
    public PropsMovementType propsMovementType;
    [Range(0.00f, 1.00f)] public float spawnChance;
}
