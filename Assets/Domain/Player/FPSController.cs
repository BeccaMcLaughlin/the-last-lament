using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;
        Vector3 move = new Vector3(getPlayerSpeed() * movement.x, -2f, getPlayerSpeed() * movement.z);
        controller.Move(move * Time.deltaTime);

        HandleMouseLook();
        HandleFOVIfSprinting();
        HandleCrouch();
    }

    bool isSprinting()
    {
        // TODO: This will get more complex to add stamina later
        return Input.GetButton("Sprint");
    }

    bool isCouching()
    {
        return Input.GetButton("Sprint");
    }

    float getPlayerSpeed()
    {
        if (isSprinting()) {
            return speed * 2f;
        }
        
        if (isCouching()) {
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
            playerCamera.transform.localPosition.y = new Vector3(playerCamera.transform.localPosition.x, cameraStandingHeight - 0.5f, playerCamera.transform.localPosition.z);
        }
        // Check if the crouch button is released
        else if (Input.GetButtonUp("Crouch"))
        {
            playerCamera.transform.localPosition.y = new Vector3(playerCamera.transform.localPosition.x, cameraStandingHeight, playerCamera.transform.localPosition.z);
        }
    }

    void HandleFOVIfSprinting()
    {
        playerCamera.fieldOfView = isSprinting() ? defaultCameraFOV * 1.1f : defaultCameraFOV;
    }
}
