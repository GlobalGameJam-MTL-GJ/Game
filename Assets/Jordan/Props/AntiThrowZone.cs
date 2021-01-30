using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiThrowZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Cant throw");
        if (other.gameObject.layer != 22) return;
        PlayerAction playerAction = other.GetComponent<PlayerAction>();
        if (playerAction != null)
            playerAction.CanThrowOrDrop = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Can throw");
        if (other.gameObject.layer != 22) return;
        PlayerAction playerAction = other.GetComponent<PlayerAction>();
        if (playerAction != null)
            playerAction.CanThrowOrDrop = true;
    }
}
