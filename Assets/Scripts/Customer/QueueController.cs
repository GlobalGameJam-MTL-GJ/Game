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

    public void RelinquishQueueSpot(QueueSpot queueSpot)
    {
        var queue = GetQueue(queueSpot);

        /*
        foreach (var _queueSpot in queue._queueSpots)
        {
            if(!_queueSpot.IsEmpty && _queueSpot != queueSpot && queueSpot.QueueOrder+1 == _queueSpot.QueueOrder)
            {
                _queueSpot.IsEmpty = true;
                OnQueueSpotOpenedUp?.Invoke(_queueSpot, queueSpot);
            }
        }
        */

        for (int i = 0; i < queue._queueSpots.Count; i++)
        {
            if (!queue._queueSpots[i].IsEmpty && queue._queueSpots[i] != queueSpot && queueSpot.QueueOrder + 1 == queue._queueSpots[i].QueueOrder)
            {
                queue._queueSpots[i].IsEmpty = true;
                OnQueueSpotOpenedUp?.Invoke(queue._queueSpots[i], queueSpot);

                for (int j = i; j >= 0; j--)
                {
                    if (!queue._queueSpots[j].IsEmpty && queue._queueSpots[j] != queueSpot && queueSpot.QueueOrder + 1 == queue._queueSpots[j].QueueOrder)
                    {
                        queue._queueSpots[j].IsEmpty = true;
                        OnQueueSpotOpenedUp?.Invoke(queue._queueSpots[j], queueSpot);
                    }
                }
            }
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
}
