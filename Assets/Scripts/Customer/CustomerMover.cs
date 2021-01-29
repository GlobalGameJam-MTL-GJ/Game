using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMover : MonoBehaviour
{
    [SerializeField] private QueueController _queueController;

    public QueueSpot _queueSpot;

    //private CustomerController _customerController;

    public Action OnCustomerWaiting;

    public static Action<QueueSpot> OnCustomerRelinquishQueueSpot;

    public void OnEnable()
    {
        //_customerController = GetComponent<CustomerController>();
        //_customerController.OnCustomerOrderComplete += RelinquishQueueSpot;
        QueueController.OnQueueSpotOpenedUp += OnQueueSpotOpenedUp;
    }

    public void OnDisable()
    {
        //_customerController.OnCustomerOrderComplete -= RelinquishQueueSpot;
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

        LeanTween.init(800);
        LeanTween.move(gameObject, _queueSpot.transform.position, 1.0f).setOnComplete(TriggerEvent);
    }

    public void TriggerEvent()
    {
        if (!_queueSpot.isQueueFront) { return; }
        OnCustomerWaiting?.Invoke();
    }
    /*
    private void RelinquishQueueSpot()
    {
        OnCustomerRelinquishQueueSpot?.Invoke(_queueSpot);
    }
    */
    private void OnQueueSpotOpenedUp(QueueSpot oldQueueSpot, QueueSpot newQueueSpot)
    {
        if(_queueSpot == oldQueueSpot)
        {
            _queueSpot = newQueueSpot;
            _queueSpot.IsEmpty = false;
            LeanTween.move(gameObject, _queueSpot.transform.position, 1.0f).setOnComplete(TriggerEvent);
        }
    }
}


