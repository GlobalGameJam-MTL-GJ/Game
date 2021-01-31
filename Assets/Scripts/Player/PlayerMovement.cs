using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float m_WalkingSpeed = 5f;
    public float WalkingSpeed { get { return this.m_WalkingSpeed; } set { this.m_WalkingSpeed = value; } }

    [SerializeField]
    private float m_RotationSpeed = 2f;

    private float speedMultiplier = 1;
    private Vector3 m_Direction;
    private Rigidbody m_rb;
    private PlayerInputs playerInputs;
    private Animator animator;

    private void Awake()
    {
        playerInputs = GetComponent<PlayerInputs>();
        m_rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        m_Direction = new Vector3(playerInputs.MovementVector.x, 0.0f, playerInputs.MovementVector.y);  
        
        if(m_Direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_Direction), Time.deltaTime * m_RotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        m_rb.velocity = m_Direction * (m_WalkingSpeed * speedMultiplier);
        animator.SetFloat("Speed", m_rb.velocity.magnitude);
    }

    public void SetSpeedModifier(float speedModifier)
    {
        speedMultiplier = 1 + speedModifier;
    }
}
