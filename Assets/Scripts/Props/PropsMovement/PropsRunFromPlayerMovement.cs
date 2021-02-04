using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PropsRunFromPlayerMovement : PropsMovement
{
    private Transform playerTransform;

    private bool goingIntoWall;

    private Vector3 lastPosition = Vector3.zero;

    private float timeWithSameDestBeforeForcingMovement = 0.5f;
    private int framesBeforeCalculatingPath = 6;
    private float currentTimer = 0f;

    private bool forcingMovement;

    private float forcingMovementTimer = 1f;

    private float currentForcingMovementTimer;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = LevelManager.instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 myPosition = transform.position;
        if (Time.frameCount % framesBeforeCalculatingPath != 0) return;
        if (!forcingMovement || navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + 0.2f)
        {
            if (navMeshAgent.enabled && Vector3.Distance(playerPosition, myPosition) < 4f)
            {
                Vector3 dest = (transform.position - playerTransform.position).normalized * 1.5f;
                if (NavMesh.SamplePosition(transform.position + dest, out NavMeshHit navHit, 6, NavMesh.AllAreas))
                {
                    if (Vector3.Distance(lastPosition, myPosition) < 0.05f)
                    {
                        currentTimer += Time.deltaTime * framesBeforeCalculatingPath;
                        if (currentTimer >= timeWithSameDestBeforeForcingMovement)
                        {
                            currentTimer = 0;
                            dest = (playerPosition - myPosition).normalized;
                            Quaternion quaternion = Quaternion.Euler(0, UnityEngine.Random.Range(-45, 45), 0);
                            dest = quaternion * dest;
                            dest *= 2f;
                            currentForcingMovementTimer = 0;
                            forcingMovement = true;
                            navMeshAgent.SetDestination(dest);
                            return;
                        }
                    }
                    else
                    {
                        currentTimer = 0;
                        lastPosition = transform.position;
                    }
                    navMeshAgent.SetDestination(navHit.position);

                }
                else
                {
                    Debug.Log("doo");
                }
            }
        }
        else
        {
            currentForcingMovementTimer += Time.deltaTime * framesBeforeCalculatingPath;
            if (currentForcingMovementTimer >= forcingMovementTimer)
            {
                forcingMovement = false;
                currentForcingMovementTimer = 0;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(lastPosition, 0.2f);
    }

    public override void ActivateMovement()
    {
        navMeshAgent.enabled = true;
    }

    public override void DeactivateMovement()
    {
        navMeshAgent.enabled = false;
    }

    public override PropsMovementType GetMovementType()
    {
        return PropsMovementType.RunFromPlayer;
    }
}
