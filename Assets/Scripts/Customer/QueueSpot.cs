using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QueueSpot : MonoBehaviour
{
    private bool isEmpty = true;

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }

    public bool isQueueFront;

    public int QueueOrder;

    public Vector3 endPoint;
    public float YAngle;
}
