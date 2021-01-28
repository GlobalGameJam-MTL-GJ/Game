using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMover : MonoBehaviour
{
    public Action OnCustomerWaiting;

    private LTSeq _seq;

    [SerializeField] private List<Transform> _waypoints;


    public void Setup(List<Transform> waypoints)
    {
        _waypoints = waypoints;
    }

    void Start()
    {
        _seq = LeanTween.sequence();
        foreach (var waypoint in _waypoints)
        {
        }
        for (int i = 0; i < _waypoints.Count; i++)
        {
            if(i == _waypoints.Count -1)
            {
                _seq.append(LeanTween.move(gameObject, _waypoints[i].position, 1.0f).setOnComplete(TriggerEvent));
            }
            else
            {
                _seq.append(LeanTween.move(gameObject, _waypoints[i].position, 1.0f));
            }
        }
    }
    public void TriggerEvent()
    {
        OnCustomerWaiting?.Invoke();
    }
}


