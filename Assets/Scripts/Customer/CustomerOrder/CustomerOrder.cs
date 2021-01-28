using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder
{
    [SerializeField] private Sprite _propImage;
    [SerializeField] private float _orderTime;

    public Sprite PropImage => _propImage;
    public float OrderTime => _orderTime;

    public CustomerOrder(CustomerOrderDefinition definition)
    {
        _propImage = definition.PropImage;
        _orderTime = definition.OrderTime;
    }
}

