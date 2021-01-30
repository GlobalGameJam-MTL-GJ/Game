using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private Customer _customer;
    [SerializeField] private Transform objectSnapTransform;
    [SerializeField] private CustomerOrderController _customerOrderController;

    public CustomerOrderController CustomerOrderController => _customerOrderController;

    public void Setup(Customer customer, CustomerOrder customerOrder, GameObject wantedPropsObject)
    {
        _customer = customer;
        _customerOrderController.Setup(customerOrder, wantedPropsObject);
    }

    public void SnapObjectToHands(GameObject objectToSnap)
    {
        objectToSnap.transform.SetParent(null);
        objectToSnap.transform.SetParent(objectSnapTransform);
        objectToSnap.transform.position = objectSnapTransform.position;
    }
}
