using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Queue
{
    public bool IsEmpty => _queueSpots.Exists(queueSpot => queueSpot.IsEmpty);

    public List<QueueSpot> _queueSpots;

    public int GetLowestAvailableSpotNumber()
    {
        int spotNumber = 0;

        foreach (var queueSpot in _queueSpots)
        {
            if (queueSpot.IsEmpty)
            {
                return spotNumber;
            }
            spotNumber++;
        }
        return int.MaxValue;
    }

    public QueueSpot GetAvailableQueueSpot()
    {
        foreach (var queueSpot in _queueSpots)
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
