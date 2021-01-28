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
    [SerializeField] private List<WayPoints> _waypointsParent;

    [SerializeField] private float _spawnRate;
    private float _runningTimer;

    void Update()
    {
        _runningTimer += Time.deltaTime;

        if(_runningTimer > _spawnRate)
        {
            _runningTimer = 0;

            var customer = new Customer(_customersDefinitions[0]);
            var customerOrder = new CustomerOrder(customer.CustomerOrders[0]);

            var rand = Random.Range(0, 3);
            var customerGO = Instantiate(customer.Model, _waypointsParent[rand].waypoints[0].position, Quaternion.identity);
            customerGO.GetComponent<CustomerMover>().Setup(_waypointsParent[rand].waypoints);
            customerGO.GetComponent<CustomerController>().Setup(customer, customerOrder);
        }
    }
}
