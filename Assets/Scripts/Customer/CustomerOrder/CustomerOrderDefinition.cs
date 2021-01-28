using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CustomerOrder", menuName = "Customer/Customer Order", order = 1)]
public class CustomerOrderDefinition : ScriptableObject
{
    [SerializeField] private Sprite _propImage;
    [SerializeField] private float _orderTime;

    public Sprite PropImage => _propImage;
    public float OrderTime => _orderTime;
}
