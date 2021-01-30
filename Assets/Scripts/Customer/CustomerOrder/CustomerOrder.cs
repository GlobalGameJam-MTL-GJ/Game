using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder
{
    [SerializeField] private PropsType _propstype;
    [SerializeField] private Sprite _propImage;
    [SerializeField] private float _orderTime;
    [SerializeField] private int _score;

    public PropsType PropsType => _propstype;
    public Sprite PropImage => _propImage;
    public float OrderTime => _orderTime;

    public int Score => _score;

    public CustomerOrder(CustomerOrderDefinition definition)
    {
        _propstype = definition.PropsType;
        _propImage = definition.PropImage;
        _orderTime = definition.OrderTime;
        _score = definition.Score;
    }
}

