using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float m_WalkingSpeed = 5f;

    [SerializeField]
    private float m_RotationSpeed = 2f;

    private Vector3 m_Direction;
    private Rigidbody m_rb;


    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Direction = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));  
        
        if(m_Direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_Direction), Time.deltaTime * m_RotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        m_rb.velocity = m_Direction * m_WalkingSpeed;
    }
}
