using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] private Canvas _customerOrderUI;
    [SerializeField] private Image _propImage;
    [SerializeField] private Image _rageBar;
    [SerializeField] private CustomerMover _customerMover;

    private CustomerOrder _customerOrder;
    private float _waitTime;

    public static Action OnCustomerOrderNotComplete;

    private bool isWaiting = false;

    private void Awake()
    {
        _customerMover.OnCustomerWaiting += OnCustomerWaiting;

    }

    private void OnDisable()
    {
        _customerMover.OnCustomerWaiting -= OnCustomerWaiting;
    }

    public void Setup(CustomerOrder customerOrder)
    {
        _customerOrder = customerOrder;
        _propImage.sprite = _customerOrder.PropImage;
    }

    private void Update()
    {
        UpdateWaitTime();
    }

    private void UpdateWaitTime()
    {
        if (!isWaiting) { return; }

        _waitTime += Time.deltaTime;

        if (_waitTime > 2.0f)//_customerOrder.OrderTime)
        {
            _customerOrderUI.enabled = true;

            //OnCustomerOrderNotComplete?.Invoke();
        }
    }

    private void OnCustomerWaiting()
    {
        isWaiting = true;
    }
}
