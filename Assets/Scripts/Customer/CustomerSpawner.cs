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

            var customer = new Customer(_customersDefinitions[0]);
            var customerOrder = new CustomerOrder(customer.CustomerOrders[0]);

            var customerGO = Instantiate(customer.Model, _spawnpoint.position, Quaternion.identity);
            customerGO.GetComponent<CustomerController>().Setup(customer, customerOrder);
        }
    }
}
