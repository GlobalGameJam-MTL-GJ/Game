using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    [SerializeField] private List<Queue> _queues;

    public bool IsSpotAvailable => _queues.Exists(queue => queue.IsEmpty);

    public static Action<QueueSpot, QueueSpot> OnQueueSpotOpenedUp;

    void OnEnable()
    {
        CustomerMover.OnCustomerRelinquishQueueSpot += RelinquishQueueSpot;
    }

    void OnDisable()
    {
        CustomerMover.OnCustomerRelinquishQueueSpot -= RelinquishQueueSpot;
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
            return _queues[(int)smallestQueue].GetAvailableQueueSpot();
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
}
