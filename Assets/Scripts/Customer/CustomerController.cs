using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private Customer _customer;

    [SerializeField] private CustomerOrderController _customerOrderController;

    public Action OnCustomerOver;

    public void Setup(Customer customer, CustomerOrder customerOrder)
    {
        _customer = customer;
        _customerOrderController.Setup(customerOrder);
    }

    void OnMouseDown()
    {
        OnCustomerOver?.Invoke();
        Destroy(gameObject);
    }
}
