using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    [SerializeField] private GameObject _customerModel;
    [SerializeField] private CustomerType _customerType;
    [SerializeField] private List<CustomerOrderDefinition> _customerOrderDefinitions;

    public GameObject Model => _customerModel;
    public CustomerType Type => _customerType;

    public List<CustomerOrderDefinition> CustomerOrders => _customerOrderDefinitions;

    public Customer(CustomerDefinition definition)
    {
        _customerModel = definition.Model;
        _customerType = definition.Type;
        _customerOrderDefinitions = definition.CustomerOrders;
    }
}
