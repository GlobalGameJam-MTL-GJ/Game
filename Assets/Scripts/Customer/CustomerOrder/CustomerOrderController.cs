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
    [SerializeField] private int pointsPerSecondsRemaining = 1;
    private GameObject wantedProps;
    public CustomerMover CustomerMover => _customerMover;

    public PropsType propsType;
    public PropsColor propsColor;

    private CustomerOrder _customerOrder;
    private float _waitTime = 0;
    private CustomerController customerComponent;
    public static Action<GameObject> OnCustomerOrderComplete;
    public static Action<GameObject> OnCustomerOrderNotComplete;
    public static Action<GameObject> OnCustomerLeaving;
    public static Action<GameObject> OnCustomerGivenWrongProp;

    public bool isWaiting = false;

    private void Awake()
    {
        _customerMover.OnCustomerWaiting += OnCustomerWaiting;
        customerComponent = GetComponentInParent<CustomerController>();
    }

    private void OnDisable()
    {
        _customerMover.OnCustomerWaiting -= OnCustomerWaiting;
    }

    public void Setup(CustomerOrder customerOrder, GameObject wantedProps)
    {
        this.wantedProps = wantedProps;
        _customerOrder = customerOrder;
        propsType = _customerOrder.PropsType;
        propsColor = _customerOrder.PropsColor;
        _propImage.sprite = _customerOrder.PropImage;
        _propImage.color = SetPropImageColor(propsColor);
    }

    private Color SetPropImageColor(PropsColor propsColor)
    {
        switch (propsColor)
        {
            case PropsColor.Orange:
                return new Color32(255, 165, 0, 255);
            case PropsColor.Purple:
                return Color.magenta;
            case PropsColor.Blue:
                return Color.blue;
            case PropsColor.Green:
                return Color.green;
            default:
                return Color.white;
        }
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
            OnCustomerLeaving?.Invoke(wantedProps);
        }
    }

    private void OnCustomerWaiting()
    {
        isWaiting = true;
    }

    public bool TryToCompleteOrder(Props props)
    {
        if (propsType == props.GetPropsType() && propsColor == props.GetPropsColor())
        {
            OnCustomerOrderComplete?.Invoke(transform.parent.gameObject);
            customerComponent.SnapObjectToHands(props.gameObject);
            ScoreManager.instance.ModifyScore(_customerOrder.Score + Mathf.RoundToInt(_customerOrder.OrderTime - _waitTime));
            return true;
        }
        else
        {
            OnCustomerGivenWrongProp?.Invoke(wantedProps);
            OnCustomerOrderNotComplete?.Invoke(transform.parent.gameObject);
            StrikeManager.instance.AddStrike();
            return false;
        }
    }
}
