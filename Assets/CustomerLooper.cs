using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLooper : MonoBehaviour
{
    [SerializeField] private List<CustomerDefinition> _customerDefinitions;

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

        _spawnedCustomer = Instantiate(customerDefinition.Model, 
            rightPoint.position, 
            Quaternion.identity);

        _spawnedCustomer.GetComponentInChildren<Animator>().SetBool("Walking", true);

        _spawnedCustomer.transform.LookAt(leftPoint);
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
}
