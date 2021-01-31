using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.AI;

public abstract class PropsMovement : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected virtual void Awake() => navMeshAgent = GetComponent<NavMeshAgent>();
    
    public abstract void ActivateMovement();
    public abstract void DeactivateMovement();

    public abstract PropsMovementType GetMovementType();
}
