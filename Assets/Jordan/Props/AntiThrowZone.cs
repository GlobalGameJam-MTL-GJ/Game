using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiThrowZone : MonoBehaviour
{
    [SerializeField] private GameObject Rooftop;
    private Camera camera;
    private void Awake()
    {
        camera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 22) return;
        camera.cullingMask |= 1 << 24;
        Rooftop.SetActive(true);
        LeanTween.cancelAll();
        LeanTween.value(gameObject, 15, 1.26f, 0.8f).setEaseOutBounce().setOnUpdate(UpdateYPosition);
        PlayerAction playerAction = other.GetComponent<PlayerAction>();
        if (playerAction != null)
            playerAction.CanThrowOrDrop = false;
    }

    private void UpdateYPosition(float value)
    {
        var position = Rooftop.transform.position;
        position = new Vector3(position.x, value, position.z);
        Rooftop.transform.position = position;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 22) return;
        camera.cullingMask &= ~(1 << 24);
        LeanTween.cancelAll();
        LeanTween.value(gameObject, 1.26f, 15, 0.6f).setOnUpdate(UpdateYPosition).setOnComplete(() => Rooftop.SetActive(false));
        PlayerAction playerAction = other.GetComponent<PlayerAction>();
        if (playerAction != null)
            playerAction.CanThrowOrDrop = true;
    }
}
