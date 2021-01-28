using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Customer/Customer", order = 1)]
public class CustomerDefinition : ScriptableObject
{
    [SerializeField] private GameObject _customerModel;
    [SerializeField] private CustomerType _customerType;
    [SerializeField] private List<CustomerOrderDefinition> _customerOrderDefinitions;

    public GameObject Model => _customerModel;
    public CustomerType Type => _customerType;

    public List<CustomerOrderDefinition> CustomerOrders => _customerOrderDefinitions;
}
