using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsAnimator : MonoBehaviour
{
    [SerializeField] private Transform lowHoverHeightTransform;
    [SerializeField] private Transform highHoverHeightTransform;


    private float lowHoverHeight = 0;
    private float highHoverHeight = 1;
    
    private bool goingUp = true;
    private int hoveringId;
    private bool hasHovered = false;
    private Props parentProps;
    private void Awake()
    {
        parentProps = GetComponentInParent<Props>();
        parentProps.OnMovementDisabled += StopHovering;
        parentProps.OnMovementEnabled += StartHovering;
        lowHoverHeight = lowHoverHeightTransform.position.y;
        highHoverHeight = highHoverHeightTransform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StopHovering(PropsMovement propsMovement)
    {
        if (propsMovement.GetType() == typeof(PropsIdleMovement)) return;
        if (!hasHovered) return;
        transform.localPosition = Vector3.zero;
        LeanTween.cancel(this.gameObject, hoveringId);
        hasHovered = false;
    }
    
    public void StartHovering(PropsMovement propsMovement)
    {
        if (propsMovement.GetType() == typeof(PropsIdleMovement)) return;
        hasHovered = true;
        LTDescr hoveringTween = LeanTween.moveLocalY(gameObject, goingUp ? highHoverHeight : lowHoverHeight, 0.5f)
            .setOnComplete(() =>
            {
                goingUp = !goingUp;
                StartHovering(propsMovement);
            });
        hoveringId = hoveringTween.id;
    }

    private void OnDestroy()
    {
        parentProps.OnMovementDisabled -= StopHovering;
        parentProps.OnMovementEnabled -= StartHovering;
    }
}
