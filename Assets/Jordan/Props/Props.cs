using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Props : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PropsMovement propsMovementComponent;
    private PropsType propsType;
    private bool pickedUp;
    private bool beingThrown;
    private Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMovementAndAssignType()
    {
        propsMovementComponent = GetComponent<PropsMovement>();
        if (propsMovementComponent != null && !pickedUp) propsMovementComponent.ActivateMovement();
    }

    public void SetPropsType(PropsType propsType)
    {
        this.propsType = propsType;
    }

    public void GetPickedUp()
    {
        if (propsMovementComponent == null) propsMovementComponent = GetComponent<PropsMovement>();
        propsMovementComponent.DeactivateMovement();
        pickedUp = true;
    }

    public void GetDropped()
    {
        propsMovementComponent.ActivateMovement();
        pickedUp = false;
    }

    public void GetThrown(Vector3 transformForward, float throwForce)
    {
        beingThrown = true;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(transformForward * throwForce, ForceMode.Impulse);
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!beingThrown) return;
        if (other.gameObject.layer == 19)
        {
            beingThrown = false;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
