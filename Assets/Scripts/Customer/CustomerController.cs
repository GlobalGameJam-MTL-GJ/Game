using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private Transform objectSnapTransform;
    [SerializeField] private CustomerOrderController _customerOrderController;

    private Customer _customer;

    public Customer Customer => _customer;
    public CustomerOrderController CustomerOrderController => _customerOrderController;

    public void Setup(Customer customer, CustomerOrder customerOrder, GameObject wantedPropsObject)
    {
        _customer = customer;
        _customerOrderController.Setup(customerOrder, wantedPropsObject);
        Instantiate(_customer.Model, transform);
    }

    public void SnapObjectToHands(GameObject objectToSnap)
    {
        objectToSnap.transform.SetParent(null);
        objectToSnap.transform.SetParent(objectSnapTransform);
        objectToSnap.transform.position = objectSnapTransform.position;
    }
}
