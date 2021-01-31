using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollisionManager : MonoBehaviour
{
    public static DoorCollisionManager instance;
    [SerializeField] private GameObject player;
    public Door[] doors;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        doors = FindObjectsOfType<Door>();
        DisableDoorCollisionWithObject(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DisableDoorCollisionWithObject(GameObject go)
    {
        foreach (var door in doors)
        {
            Physics.IgnoreCollision(go.GetComponent<Collider>(), door.GetComponent<Collider>(), true);
        }
    }
    
    public void EnableDoorCollisionWithObject(GameObject go)
    {
        foreach (var door in doors)
        {
            Physics.IgnoreCollision(go.GetComponent<Collider>(), door.GetComponent<Collider>(), false);
        }
    }
}
