using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    private InputAction mousePositionAction;
    private InputAction mouseAction;

    public static Vector2 mousePosition;
    public static bool wasMousePressed;
    public static bool wasMouseReleased;
    public static bool isMousePressed;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        mousePositionAction = playerInput.actions["Mouse Position"];
        mouseAction = playerInput.actions["Mouse"];
    }

    private void Update()
    {
        mousePosition = mousePositionAction.ReadValue<Vector2>();

        wasMousePressed = mouseAction.WasPressedThisFrame();
        wasMouseReleased = mouseAction.WasReleasedThisFrame();
        isMousePressed = mouseAction.IsPressed();
    }
}
