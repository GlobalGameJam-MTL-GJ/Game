using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PropsRandomMovement : PropsMovement
{
    private float distanceThreshold = 1f;
    private float distanceRemaining;
    private bool isWandering;
    
    private void Awake()
    {
        base.Awake();
        if (distanceThreshold < navMeshAgent.stoppingDistance)
            distanceThreshold = navMeshAgent.stoppingDistance + 0.3f;
    }


    private void WanderToRandomPoint()
    {
        Vector3 dest = GameUtilities.GetRandomPointInBox(LevelManager.instance.PropsBounds);
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(dest, out navHit, 8, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(navHit.position);
            isWandering = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWandering) return;
        
        if(navMeshAgent.remainingDistance <= distanceThreshold)
            WanderToRandomPoint();
    }

    public override void ActivateMovement()
    {
        navMeshAgent.enabled = true;
        if(!isWandering)
            WanderToRandomPoint();
    }

    public override void DeactivateMovement()
    {
        isWandering = false;
        navMeshAgent.enabled = false;
    }

    public override PropsMovementType GetMovementType()
    {
        return PropsMovementType.RandomMovement;
    }
}
