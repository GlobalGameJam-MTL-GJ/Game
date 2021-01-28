using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Props : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PropsMovement propsMovementComponent;
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

    public void ActivateMovement()
    {
        //roll and add the propsmovement
        gameObject.AddComponent(LevelManager.instance.GetPropsMovementAccordingToLevelConfig());
        propsMovementComponent = GetComponent<PropsMovement>();
        if (propsMovementComponent != null) propsMovementComponent.ActivateMovement();
    }
}
