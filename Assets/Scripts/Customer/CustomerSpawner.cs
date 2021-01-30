using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float _runningTimer;

    [SerializeField] private QueueController _queueController;

    void Update()
    {
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        if (!_queueController.IsSpotAvailable) { return; }

        _runningTimer += Time.deltaTime;

        if (_runningTimer > _spawnRate)
        {
            _runningTimer = 0;

            //get a props from the props builder
            //search for a model that wants this props
            //create the order according to the choosen props from propsbuilder
            ActivePropsEntry choosenPropsEntry = PropsBuilder.instance.GetAnActiveProps();

            var customer = new Customer(FindCustomerForTheChoosenProps(choosenPropsEntry, out var customerOrderDefinition));
            var customerOrder = new CustomerOrder(customerOrderDefinition);

            var customerGO = Instantiate(customer.Model, _spawnpoint.position, Quaternion.identity);
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
        return choosenCustomer;
    }
}
