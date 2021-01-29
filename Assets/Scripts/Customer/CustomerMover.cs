using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMover : MonoBehaviour
{
    public Action OnCustomerWaiting;

    [SerializeField] private QueueController _queueController;

    private QueueSpot _queueSpot;

    private CustomerController _customerController;

    public static Action<QueueSpot> OnCustomerRelinquishQueueSpot;

    public void OnEnable()
    {
        _customerController = GetComponent<CustomerController>();
        _customerController.OnCustomerOver += RelinquishQueueSpot;
        QueueController.OnQueueSpotOpenedUp += OnQueueSpotOpenedUp;

    }

    public void OnDisable()
    {
        _customerController.OnCustomerOver -= RelinquishQueueSpot;
        QueueController.OnQueueSpotOpenedUp -= OnQueueSpotOpenedUp;

    }

    void Start()
    {
        _queueController = FindObjectOfType<QueueController>();

        MoveCustomerToQueue();
    }

    private void MoveCustomerToQueue()
    {
        _queueSpot = _queueController.GetAvailableSpot();

        LeanTween.move(gameObject, _queueSpot.transform.position, 1.0f).setOnComplete(TriggerEvent);
    }

    public void TriggerEvent()
    {
        if (!_queueSpot.isQueueFront) { return; }
        OnCustomerWaiting?.Invoke();
    }

    private void RelinquishQueueSpot()
    {
        OnCustomerRelinquishQueueSpot?.Invoke(_queueSpot);
    }

    private void OnQueueSpotOpenedUp(QueueSpot oldQueueSpot, QueueSpot newQueueSpot)
    {
        if(_queueSpot == oldQueueSpot)
        {
            //OnCustomerRelinquishQueueSpot?.Invoke(_queueSpot);
            _queueSpot = newQueueSpot;
            _queueSpot.IsEmpty = false;
            LeanTween.move(gameObject, _queueSpot.transform.position, 1.0f).setOnComplete(TriggerEvent);
        }
    }
}


