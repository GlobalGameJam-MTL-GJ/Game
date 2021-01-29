using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Props Related Settings")] 
    public int maximumPropsAtOnce = 6;
    public List<PropsPossibleMovement> propsPossibleMovements = new List<PropsPossibleMovement>();
    
}
