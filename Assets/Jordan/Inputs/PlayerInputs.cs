using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private PlayerMovement playerMovement;

    public Vector2 MovementVector { get; private set; }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        MovementVector = Vector3.zero;
        playerInputActions.Player.Move.performed += HandleMovement;
        playerInputActions.Player.Enable();
    }

    private void HandleMovement(InputAction.CallbackContext obj)
    {
        MovementVector = obj.ReadValue<Vector2>();
    }

    public bool CheckIfMainActionButtonPressed()
    {
        return playerInputActions.Player.MainAction.triggered;
    }
    
    public bool CheckIfThrowButtonPressed()
    {
        return playerInputActions.Player.Throw.triggered;
    }
}
