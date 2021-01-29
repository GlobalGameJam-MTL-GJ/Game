using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Props Related Settings")] 
    public int maximumPropsAtOnce = 6;
    [Header("Props Movement Settings")]
    public List<PropsPossibleMovement> propsPossibleMovements = new List<PropsPossibleMovement>();
    [Header("Props Meshes Settings")]
    public List<PropsPossibleTypes> propsPossibleTypes = new List<PropsPossibleTypes>();
}
