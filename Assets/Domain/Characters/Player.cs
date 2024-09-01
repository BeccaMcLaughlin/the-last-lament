using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour, ISprinting
{

    // TODO: Move these to game settings file
    public float mouseSensitivity = 100f;
    public float defaultCameraFOV = 60f;

    private CharacterController controller;
    private Camera playerCamera;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float speed = 3f;
    private float cameraStandingHeight;

    // TODO: Will want to split this out into PlayerStamina once we add monsters
    private float staminaRegenRate = 5f;
    private float maxStamina = 100f;
    private float currentStamina = 100f;
    private bool staminaIsRegenerating = false;
    private Coroutine staminaRegenCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraStandingHeight = playerCamera.transform.localPosition.y;
    }

    void Update()
    {
        Debug.Log(currentStamina);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;
        Vector3 move = new Vector3(currentSpeed() * movement.x, -2f, currentSpeed() * movement.z);
        controller.Move(move * Time.deltaTime);

        HandleMouseLook();
        HandleFOVIfSprinting();
        HandleCrouch();

        // TODO: Will want to split this out into PlayerStamina once we add monsters
        HandleStaminaDrainFromSprinting();
    }

    public bool isSprinting()
    {
        return Input.GetButton("Sprint") && currentStamina > 0f;
    }

    bool isCrouching()
    {
        return Input.GetButton("Sprint");
    }

    public float currentSpeed()
    {
        if (isSprinting()) {
            return speed * 2f;
        }
        
        if (isCrouching()) {
            return speed * 0.5f;
        }

        return speed;
    }

    void HandleMouseLook()
    {
        // Get mouse input values for X and Y axes
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the vertical rotation and clamp it to prevent looking too far up or down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the entire player object, including the camera
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y + mouseX, 0f);
    }

    void HandleCrouch()
    {
        // Constrain the updates here to a single frame
        if (Input.GetButtonDown("Crouch"))
        {
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, cameraStandingHeight - 0.5f, playerCamera.transform.localPosition.z);
        }
        // Check if the crouch button is released
        else if (Input.GetButtonUp("Crouch"))
        {
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, cameraStandingHeight, playerCamera.transform.localPosition.z);
        }
    }

    void HandleFOVIfSprinting()
    {
        playerCamera.fieldOfView = isSprinting() ? defaultCameraFOV * 1.1f : defaultCameraFOV;
    }

    void HandleStaminaDrainFromSprinting()
    {
        if (isSprinting())
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
        else if (!Input.GetButton("Sprint") && currentStamina < maxStamina)
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
