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

    public CustomerMover CustomerMover => _customerMover;

    public PropsType propsType;

    private CustomerOrder _customerOrder;
    private float _waitTime = 0;

    public static Action<GameObject> OnCustomerOrderComplete;
    public static Action<GameObject> OnCustomerOrderNotComplete;

    public bool isWaiting = false;

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
        propsType = _customerOrder.PropsType;
        _propImage.sprite = _customerOrder.PropImage;
    }

    private void Update()
    {
        UpdateWaitTime();
    }

    private void UpdateWaitTime()
    {
        if (!isWaiting) { return; }

        _customerOrderUI.enabled = true;

        _waitTime += Time.deltaTime;

        var rect = _rageBar.rectTransform;

        rect.sizeDelta = new Vector2((float) _waitTime / _customerOrder.OrderTime, 0.25f);

        if (_waitTime > _customerOrder.OrderTime)
        {
            isWaiting = false;
            _customerOrderUI.enabled = false;
            OnCustomerOrderNotComplete?.Invoke(gameObject.transform.parent.gameObject);
        }
    }

    private void OnCustomerWaiting()
    {
        isWaiting = true;
    }

    public bool TryToCompleteOrder(Props props)
    {
        Destroy(props.gameObject);
        OnCustomerOrderComplete?.Invoke(transform.parent.gameObject);
        return true;
        /*
        if (props.GetPropsType() == propsType)
        {
            OnCustomerOrderComplete?.Invoke(transform.parent.gameObject);
        }
        */
    }
}
