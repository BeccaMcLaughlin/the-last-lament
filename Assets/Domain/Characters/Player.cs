using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class FPSController : MonoBehaviour
{
    public float defaultCameraFOV = 60f;
    public float currentStamina = 100f;

    private Camera playerCamera;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float cameraStandingHeight;

    // TODO: Will want to split this out into PlayerStamina once we add monsters
    private float staminaRegenRate = 5f;
    private float maxStamina = 100f;
    private bool staminaIsRegenerating = false;
    private Coroutine staminaRegenCoroutine;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerMovement = GetComponent<PlayerMovement>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraStandingHeight = playerCamera.transform.localPosition.y;
    }
    
    void OnEnable()
    {
        PlayerActions.Instance.OnCrouchPressed += HandleCrouchPressed;
        PlayerActions.Instance.OnCrouchReleased += HandleCrouchReleased;
    }

    void OnDisable()
    {
        PlayerActions.Instance.OnCrouchPressed -= HandleCrouchPressed;
        PlayerActions.Instance.OnCrouchReleased -= HandleCrouchReleased;
    }

    void Update()
    {
        playerMovement.Move();

        HandleMouseLook();
        HandleFOVIfSprinting();

        // TODO: Will want to split this out into PlayerStamina once we add monsters
        HandleStaminaDrainFromSprinting();
    }

    void HandleMouseLook()
    {
        Vector2 lookValue = PlayerActions.Instance.LookValue;
        
        // Get mouse input values for X and Y axes
        float mouseX = lookValue.x * Time.deltaTime;
        float mouseY = lookValue.y * Time.deltaTime;

        // Adjust the vertical rotation and clamp it to prevent looking too far up or down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the entire player object, including the camera
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y + mouseX, 0f);
    }

    public void HandleCrouchPressed()
    {
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, cameraStandingHeight - 0.5f, playerCamera.transform.localPosition.z);
    }

    public void HandleCrouchReleased()
    {
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, cameraStandingHeight, playerCamera.transform.localPosition.z);
    }

    void HandleFOVIfSprinting()
    {
        playerCamera.fieldOfView = playerMovement.isSprinting() ? defaultCameraFOV * 1.1f : defaultCameraFOV;
    }

    void HandleStaminaDrainFromSprinting()
    {
        if (playerMovement.isSprinting())
        {
            // Cancel stamina regeneration if the player starts sprinting again
            if (staminaRegenCoroutine != null)
            {
                StopCoroutine(staminaRegenCoroutine);
                staminaRegenCoroutine = null;
                staminaIsRegenerating = false;
            }

            // Drain stamina at a rate of 10 units per second
            currentStamina = Mathf.Clamp(currentStamina - (10f * Time.deltaTime), 0, maxStamina);
        }
        // Regenerate stamina when not sprinting and not at max stamina
        else if (!playerMovement.isSprinting() && currentStamina < maxStamina)
        {
            // Start the regeneration delay if it's not already in progress
            if (!staminaIsRegenerating && staminaRegenCoroutine == null)
            {
                staminaRegenCoroutine = StartCoroutine(StaminaRegenDelay());
            }
        }
    }

    private IEnumerator StaminaRegenDelay()
    {
        yield return new WaitForSeconds(4f); // Delay before stamina starts regenerating
        staminaIsRegenerating = true; // Allow regeneration to begin

        // Start regenerating stamina continuously after the delay
        while (currentStamina < maxStamina)
        {
            currentStamina = Mathf.Clamp(currentStamina + staminaRegenRate * Time.deltaTime, 0, maxStamina);
            yield return null;
        }

        // Stop regeneration once stamina is full
        staminaRegenCoroutine = null;
        staminaIsRegenerating = false;
    }
}
