using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsFollowPlayerMovement : PropsMovement
{
    private Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = LevelManager.instance.TempPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 4 == 0 && navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }
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
        return PropsMovementType.FollowPlayer;
    }
}
