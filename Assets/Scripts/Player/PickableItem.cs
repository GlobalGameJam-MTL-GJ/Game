using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    private Rigidbody m_rb;
    public Rigidbody Rb{get; set; }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }
}
