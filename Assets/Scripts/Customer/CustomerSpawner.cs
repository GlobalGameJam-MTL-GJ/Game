using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WayPoints
{
    public List<Transform> waypoints;
}

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<CustomerDefinition> _customersDefinitions;
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private float _spawnRate;

    [SerializeField] private QueueController _queueController;
    [SerializeField] private PropsBuilder _propsBuilder;

    private float _runningTimer;

    void Update()
    {
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        if (!_queueController.IsSpotAvailable && !_propsBuilder.PropsAvailable) { return; }

        _runningTimer += Time.deltaTime;

        if (_runningTimer > _spawnRate)
        {
            _runningTimer = 0;

            var propGO = _propsBuilder.GetAnActiveProps();
            var propsType = propGO.GetComponent<Props>().GetPropsType();

            var (cutomerDefinition, customerOrderDefinition) = MapPropTypeToACustomer(propsType);

            var customer = new Customer(cutomerDefinition);//Customer(_customersDefinitions[0]);
            var customerOrder = new CustomerOrder(customerOrderDefinition);//CustomerOrder(customer.CustomerOrders[0]);

            var customerGO = Instantiate(customer.Model, _spawnpoint.position, Quaternion.identity);
            customerGO.GetComponent<CustomerController>().Setup(customer, customerOrder);
        }
    }

    private (CustomerDefinition,CustomerOrderDefinition) MapPropTypeToACustomer(PropsType propsType)
    {
        //Block runs for Potion proptypes only
        if(propsType == PropsType.Potion)
        {
            var randCustomer = UnityEngine.Random.Range(0, _customersDefinitions.Count);

            var customerDefinition = _customersDefinitions[randCustomer];

            foreach(var customerOrderDefinition in customerDefinition.CustomerOrders)
            {
                if(customerOrderDefinition.PropsType == PropsType.Potion)
                {
                    return (customerDefinition, customerOrderDefinition);
                }
            }
        }

        //For all other proptypes
        foreach (var customerDefinition in _customersDefinitions)
        {
            foreach (var customerOrderDefinition in customerDefinition.CustomerOrders)
            {
                if(propsType == customerOrderDefinition.PropsType)
                {
                    return (customerDefinition, customerOrderDefinition);
                }
            }
        }
        return (null,null);
    }
}
