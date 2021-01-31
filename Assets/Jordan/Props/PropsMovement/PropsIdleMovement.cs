using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsIdleMovement : PropsMovement
{
    public override void ActivateMovement()
    {
    }

    public override void DeactivateMovement()
    {
    }

    public override PropsMovementType GetMovementType()
    {
        return PropsMovementType.Idle;
    }
}
