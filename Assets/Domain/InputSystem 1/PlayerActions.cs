using UnityEngine;

public class PlayerActions
{
    private static PlayerActions _instance;
    public static PlayerActions Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerActions();
            return _instance;
        }
    }

    private InputActions inputActions;

    // Private constructor
    private PlayerActions()
    {
        inputActions = new InputActions();
        inputActions.Enable();
    }

    public bool IsSprinting()
    {
        return inputActions.Gameplay.Sprint.IsPressed();
    }

    public bool IsCrouching()
    {
        return inputActions.Gameplay.Crouch.IsPressed();
    }

    public Vector2 GetLookValue()
    {
        return inputActions.Gameplay.Look.ReadValue<Vector2>();
    }

    public Vector2 GetMoveValue()
    {
        return inputActions.Gameplay.Move.ReadValue<Vector2>();
    }

    public bool IsInteracting()
    {
        return inputActions.Gameplay.Interact.IsPressed();
    }

    public bool IsExiting()
    {
        return inputActions.Gameplay.Exit.IsPressed();
    }

    public int GetScrollValue()
    {
        return inputActions.Gameplay.Scroll.ReadValue<int>();
    }

    public bool IsDiscarding()
    {
        return inputActions.Gameplay.Discard.IsPressed();
    }

    public void DisableInputs()
    {
        inputActions.Disable();
    }

    public void EnableInputs()
    {
        inputActions.Enable();
    }
}