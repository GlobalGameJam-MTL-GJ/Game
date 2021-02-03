using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLooper : MonoBehaviour
{
    [SerializeField] private List<CustomerDefinition> _customerDefinitions;
    [SerializeField] private List<GameObject> _props;
    [SerializeField] private GameObject _customerBase;

    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    private int _currentCustomerDefinitionIndex = 0;
    private GameObject _spawnedCustomer;

    private float _runningTimer;
    private float _spawnRate = 10f;

    private bool isCustomerAngry = false;

    void Update()
    {
        _runningTimer += Time.deltaTime;

        if (_runningTimer > _spawnRate)
        {
            _runningTimer = 0;

            if(isCustomerAngry)
            {
                SpawnAngryCustomer();
                isCustomerAngry = false;
            }
            else
            {
                SpawnHappyCustomer();
                isCustomerAngry = true;
            }
        }
    }

    private void SpawnAngryCustomer()
    {
        if (_currentCustomerDefinitionIndex >= _customerDefinitions.Count)
        {
            _currentCustomerDefinitionIndex = 0;
        }

        var customerDefinition = _customerDefinitions[_currentCustomerDefinitionIndex];

        _spawnedCustomer = Instantiate(customerDefinition.Model,
            leftPoint.position,
            Quaternion.identity);

        _spawnedCustomer.GetComponentInChildren<Animator>().SetBool("Walking", true);

        _spawnedCustomer.transform.LookAt(rightPoint);
        _spawnedCustomer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        LeanTween.move(_spawnedCustomer, rightPoint, 5f).destroyOnComplete = true;
        LeanTween.delayedCall(1f, () =>
        {
            AkSoundEngine.SetSwitch("NPCType", customerDefinition.Type.ToString(), gameObject);
            AkSoundEngine.SetSwitch("NPCMood", "Angry", gameObject);
            AkSoundEngine.PostEvent("NPC_Voice", gameObject);
        });
        _currentCustomerDefinitionIndex++;
    }

    private void SpawnHappyCustomer()
    {
        if(_currentCustomerDefinitionIndex >= _customerDefinitions.Count)
        {
            _currentCustomerDefinitionIndex = 0;
        }

        var customerDefinition = _customerDefinitions[_currentCustomerDefinitionIndex];
        var orderIndex = UnityEngine.Random.Range(0, customerDefinition.CustomerOrders.Count);
        var customerDefinitionOrder = customerDefinition.CustomerOrders[orderIndex];
        var customer = new Customer(customerDefinition);
        var customerOrder = new CustomerOrder(customerDefinitionOrder);

        _spawnedCustomer = Instantiate(_customerBase,
            rightPoint.position,
            Quaternion.identity);
        _spawnedCustomer.GetComponent<CustomerController>().Setup(customer, customerOrder, null);
        var propPrefab = GetPropForHappyCustomer(customerDefinitionOrder.PropsType);
        var spawnedProp = Instantiate(propPrefab, _spawnedCustomer.transform.position, Quaternion.identity);
        var propAnim = spawnedProp.transform.GetComponentInChildren<PropsAnimator>();
        if (propAnim) { propAnim.enabled = false; }
        spawnedProp.transform.localScale = new Vector3(1f, 1f, 1f);

        _spawnedCustomer.GetComponent<CustomerController>().SnapObjectToHands(spawnedProp);
        _spawnedCustomer.GetComponentInChildren<Animator>().SetBool("Walking", true);
        _spawnedCustomer.GetComponentInChildren<Animator>().SetBool("HasObj", true);

        _spawnedCustomer.transform.LookAt(leftPoint);
        _spawnedCustomer.transform.localRotation = new Quaternion(0f, 90f, 0f, 0f);
        _spawnedCustomer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        LeanTween.move(_spawnedCustomer, leftPoint, 5f).destroyOnComplete = true;
        LeanTween.delayedCall(1f, () =>
        {
            AkSoundEngine.SetSwitch("NPCType", customerDefinition.Type.ToString(), gameObject);
            AkSoundEngine.SetSwitch("NPCMood", "Happy", gameObject);
            AkSoundEngine.PostEvent("NPC_Voice", gameObject);
        });

        _currentCustomerDefinitionIndex++;
    }

    private GameObject GetPropForHappyCustomer(PropsType propsType)
    {
        foreach (var prop in _props)
        {
            if(prop.name == propsType.ToString())
            {
                return prop;
            }
        }
        return null;
    }

}
