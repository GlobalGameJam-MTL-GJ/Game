using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public abstract class PropsMovement : MonoBehaviour
{
    public abstract void ActivateMovement();
    public abstract void DeactivateMovement();
}
