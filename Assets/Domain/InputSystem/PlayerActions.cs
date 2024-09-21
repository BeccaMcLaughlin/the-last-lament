using UnityEngine;
using UnityEngine.InputSystem;

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

    private readonly InputActions inputActions;
    
    public float MouseSensitivity { get; set; } = 1.0f;
    
    public event System.Action OnCrouchPressed;
    public event System.Action OnCrouchReleased;

    // Private constructor
    private PlayerActions()
    {
        inputActions = new InputActions();
        
        inputActions.Gameplay.Crouch.performed += CrouchPerformed;
        inputActions.Gameplay.Crouch.canceled += CrouchCanceled;

        inputActions.Enable();
    }
    
    private void CrouchPerformed(InputAction.CallbackContext context)
    {
        OnCrouchPressed?.Invoke();
    }

    // Callback when crouch is canceled (button released)
    private void CrouchCanceled(InputAction.CallbackContext context)
    {
        OnCrouchReleased?.Invoke();
    }

    public bool IsSprinting => inputActions.Gameplay.Sprint.IsPressed();

    public bool IsCrouching => inputActions.Gameplay.Crouch.IsPressed();

    public Vector2 LookValue => inputActions.Gameplay.Look.ReadValue<Vector2>() * MouseSensitivity;

    public Vector2 MoveValue => inputActions.Gameplay.Move.ReadValue<Vector2>();

    public bool IsInteracting => inputActions.Gameplay.Interact.IsPressed();

    public bool IsExiting => inputActions.Gameplay.Exit.IsPressed();

    public int ScrollValue => inputActions.Gameplay.Scroll.ReadValue<int>();

    public bool IsDiscarding => inputActions.Gameplay.Discard.IsPressed();

    public void DisableInputs()
    {
        inputActions.Disable();
    }

    public void EnableInputs()
    {
        inputActions.Enable();
    }
}