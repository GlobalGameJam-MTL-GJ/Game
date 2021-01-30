using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CustomerOrder", menuName = "Customer/Customer Order", order = 1)]
public class CustomerOrderDefinition : ScriptableObject
{
    [SerializeField] private PropsType _propsType;
    [SerializeField] private PropsColor _propsColor;
    [SerializeField] private Sprite _propImage;
    [SerializeField] private float _orderTime;
    [SerializeField] private int _score;

    public PropsType PropsType => _propsType;
    public PropsColor PropsColor { get => _propsColor; set => _propsColor = value; }
    public Sprite PropImage => _propImage;
    public float OrderTime => _orderTime;
    public int Score => _score;
}
