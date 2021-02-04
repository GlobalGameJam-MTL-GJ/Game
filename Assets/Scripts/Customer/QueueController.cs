using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    [SerializeField] private List<Queue> _queues;
    [SerializeField] private Transform getOutByBottomPoint;
    [SerializeField] private float yAngleForBottom;
    [SerializeField] private Transform getOutByTopPoint;
    [SerializeField] private float yAngleForTop;
    public bool IsSpotAvailable => _queues.Exists(queue => queue.IsEmpty);

    public static Action<QueueSpot, QueueSpot> OnQueueSpotOpenedUp;

    public Transform endPoint;

    void OnEnable()
    {
        CustomerOrderController.OnCustomerOrderNotComplete += OnCustomerOrderNotComplete;
        CustomerOrderController.OnCustomerOrderComplete += OnCustomerOrderNotComplete;
    }

    void OnDisable()
    {
        CustomerOrderController.OnCustomerOrderNotComplete -= OnCustomerOrderNotComplete;
        CustomerOrderController.OnCustomerOrderComplete -= OnCustomerOrderNotComplete;
    }


    public QueueSpot GetAvailableSpot()
    {
        var lowestSpotAvailable = int.MaxValue;
        int? smallestQueue = null;

        for (int i = 0; i < _queues.Count; i++)
        {
            if(_queues[i].IsEmpty)
            {
                int spotAvailable = _queues[i].GetLowestAvailableSpotNumber();
                if (spotAvailable < lowestSpotAvailable)
                {
                    lowestSpotAvailable = spotAvailable;
                    smallestQueue = i;
                }
            }
        }
        if(smallestQueue != null)
        {
            QueueSpot queueSpot = _queues[(int) smallestQueue].GetAvailableQueueSpot();
            queueSpot.YAngle = smallestQueue < 1 ? yAngleForBottom : yAngleForTop;
            queueSpot.endPoint = smallestQueue < 1 ? getOutByBottomPoint.position : getOutByTopPoint.position;
            return queueSpot;
        }
        return null;
    }

    private int? GetQueueSpotIndex(QueueSpot queueSpot)
    {
        var queue = GetQueue(queueSpot);

        for (int i = 0; i < queue._queueSpots.Count; i++)
        {
            if(queueSpot == queue._queueSpots[i])
            {
                return i;
            }
        }
        return null;
    }

    public void RelinquishQueueSpot(QueueSpot queueSpot)
    { 
        var queue = GetQueue(queueSpot);

        int index = queue._queueSpots.Count;
        
        if(queue.GetFullSpots() == 1)
        {
            queueSpot.IsEmpty = true;
        }

        for (int i = 0; i < queue._queueSpots.Count; i++)
        {
            if (!queue._queueSpots[i].IsEmpty && queue._queueSpots[i] != queueSpot && queueSpot.QueueOrder + 1 == queue._queueSpots[i].QueueOrder)
            {
                index = i;
            }
        }

        for (int j = index; j < queue._queueSpots.Count; j++)
        {
            //Debug.Log($"{queue._queueSpots[j].QueueOrder}");
            queue._queueSpots[j].IsEmpty = true;
            OnQueueSpotOpenedUp?.Invoke(queue._queueSpots[j], queue._queueSpots[j-1]);
        }
    }

    private Queue GetQueue(QueueSpot _queueSpot)
    {
        foreach (var queue in _queues)
        {
            foreach (var queueSpot in queue._queueSpots)
            {
                if (_queueSpot == queueSpot)
                {
                    return queue;
                }
            }
        }
        return null;
    }

    private QueueSpot GetLowestQueueSpot(Queue _queue)
    {
        foreach (var queueSpot in _queue._queueSpots)
        {
            if(queueSpot.IsEmpty)
            {
                queueSpot.IsEmpty = false;
                return queueSpot;
            }
        }
        return null;
    }

    private void OnCustomerOrderNotComplete(GameObject customerGO)
    {
        var queueSpot = customerGO.GetComponent<CustomerMover>()._queueSpot;
        customerGO.transform.GetChild(0).GetComponent<CustomerOrderController>().isWaiting = false;
        RelinquishQueueSpot(queueSpot);
        LeanTween.rotateAround(customerGO, customerGO.transform.up, queueSpot.YAngle, 0.6f).setOnComplete(() =>
        {
            Animator animator = customerGO.GetComponentInChildren<Animator>();
            animator.SetBool("Walking", true);
            animator.SetBool("HasObj", true);
            LeanTween.move(customerGO, queueSpot.endPoint, 3.5f).setOnComplete(() => { Destroy(customerGO); });
        });
    }
}
