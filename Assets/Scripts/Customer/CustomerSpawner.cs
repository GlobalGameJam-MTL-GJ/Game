using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public class WayPoints
{
    public List<Transform> waypoints;
}

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<CustomerDefinition> _customersDefinitions;
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private GameObject customerBase;
    [SerializeField] private float _spawnRate;
    private float _runningTimer;
    private bool gameStarted = false;
    [SerializeField] private QueueController _queueController;

    private void Start() => TimeManager.instance.OnGameStart += HandleGameStart;

    private void HandleGameStart() => gameStarted = true;

    void Update()
    {
        if (!gameStarted) return;
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        if (!_queueController.IsSpotAvailable) { return; }

        _runningTimer += Time.deltaTime;

        if (_runningTimer > _spawnRate)
        {
            _runningTimer = 0;

            ActivePropsEntry choosenPropsEntry = PropsBuilder.instance.GetAnActiveProps();
            
            var customer = new Customer(FindCustomerForTheChoosenProps(choosenPropsEntry, out var customerOrderDefinition));
            var customerOrder = new CustomerOrder(customerOrderDefinition);

            var customerGO = Instantiate(customerBase, _spawnpoint.position, Quaternion.identity);
            customerGO.GetComponent<CustomerController>().Setup(customer, customerOrder, choosenPropsEntry.activeProps);
        }
    }

    private CustomerDefinition FindCustomerForTheChoosenProps(ActivePropsEntry choosenPropsEntry, out CustomerOrderDefinition customerOrderDefinition)
    {
        PropsType propsType = choosenPropsEntry.propsType;
        List<CustomerDefinition> filteredCustomerDefinitions = new List<CustomerDefinition>();
        //chooses the customer
        foreach (var customerDefinition in _customersDefinitions)
        {
            foreach (var customerOrder in customerDefinition.CustomerOrders)
            {
                if (customerOrder.PropsType != propsType) continue;
                filteredCustomerDefinitions.Add(customerDefinition);
                break;
            }
        }
        //randomly chooses the order definition amongst the correct props type
        int rand = Random.Range(0, filteredCustomerDefinitions.Count);
        
        if (filteredCustomerDefinitions.Count == 0)
        {
            Debug.LogError("NO CUSTOMER WERE INTERESTED IN THE " + propsType);
        }
        
        CustomerDefinition choosenCustomer = filteredCustomerDefinitions[rand];
        List<CustomerOrderDefinition> filteredCustomerOrderDefinitions = new List<CustomerOrderDefinition>();
        foreach (var customerOrderDef in choosenCustomer.CustomerOrders)
        {
            if (customerOrderDef.PropsType != propsType) continue;
            filteredCustomerOrderDefinitions.Add(customerOrderDef);
        }

        rand = Random.Range(0, filteredCustomerOrderDefinitions.Count);
        customerOrderDefinition = filteredCustomerOrderDefinitions[rand];
        customerOrderDefinition.PropsColor = choosenPropsEntry.propsColor;
        return choosenCustomer;
    }
    
    private void OnDestroy()
    {
        if(TimeManager.instance != null)
            TimeManager.instance.OnGameStart -= HandleGameStart;
    }
}
