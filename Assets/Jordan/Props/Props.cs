using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Props : MonoBehaviour
{
    [SerializeField] private float delayAfterThrown = 2f;
    private float speedModifier;
    private float currentDelayTimer;
    private bool recoveringFromThrow;
    private NavMeshAgent navMeshAgent;
    private PropsMovement propsMovementComponent;
    private PropsType propsType;
    private PropsColor propsColor;
    private bool pickedUp;
    private bool beingThrown;
    private bool beingDropped;
    private Rigidbody rigidbody;

    public event Action<PropsMovement> OnMovementEnabled;
    public event Action<PropsMovement> OnMovementDisabled;

    public float SpeedModifier => speedModifier;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!recoveringFromThrow) return;
        currentDelayTimer -= Time.deltaTime;
        if (currentDelayTimer <= 0)
        {
            recoveringFromThrow = false;
            ActivateMovement();
        }
    }
    
    public void ActivateMovement()
    {
        propsMovementComponent = GetComponent<PropsMovement>();
        if (propsMovementComponent != null && !pickedUp)
        {
            propsMovementComponent.ActivateMovement();
            OnMovementEnabled?.Invoke(propsMovementComponent);
        }
    }

    public void SetPropsTypeAndColorAndSpeedModifier(PropsType propsType, PropsColor color, float speedModifier)
    {
        this.speedModifier = speedModifier;
        this.propsType = propsType;
        propsColor = color;
        GetComponentInChildren<MeshRenderer>().material.color = PropsBuilder.instance.PropsColorsToColors[propsColor];
    }

    public PropsType GetPropsType()
    {
        return propsType;
    }
    public PropsColor GetPropsColor()
    {
        return propsColor;
    }

    public void GetPickedUp()
    {
        if (propsMovementComponent == null) propsMovementComponent = GetComponent<PropsMovement>();
        OnMovementDisabled?.Invoke(propsMovementComponent);
        propsMovementComponent.DeactivateMovement();
        pickedUp = true;
    }

    public void GetDropped()
    {
        beingDropped = true;
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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
        if (beingThrown)
        {
            if (other.gameObject.layer == 19)
            {
                beingThrown = false;
                pickedUp = false;
                currentDelayTimer = delayAfterThrown;
                recoveringFromThrow = true;
                rigidbody.isKinematic = true;
                rigidbody.constraints = RigidbodyConstraints.None;
            }
        }
        else if (beingDropped)
        {
            if (other.gameObject.layer == 19)
            {
                beingDropped = false;
                pickedUp = false;
                rigidbody.isKinematic = true;
                rigidbody.constraints = RigidbodyConstraints.None;
                propsMovementComponent.ActivateMovement();
            }
        }
        
    }
}

