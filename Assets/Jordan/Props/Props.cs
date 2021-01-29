using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Props : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PropsMovement propsMovementComponent;
    private PropsType propsType;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMovementAndAssignType()
    {
        propsMovementComponent = GetComponent<PropsMovement>();
        if (propsMovementComponent != null) propsMovementComponent.ActivateMovement();
    }

    public void SetPropsType(PropsType propsType)
    {
        this.propsType = propsType;
    }
}
