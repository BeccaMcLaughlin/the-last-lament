using UnityEngine;

public class Corpse : MonoBehaviour, IInteractable
{
    private bool isBeingDragged = false;
    private FPSController playerController; // Reference to the player controller
    private Rigidbody rb; // Reference to the Rigidbody component

    void Start()
    {
        // Get the player controller
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
        // Get the Rigidbody component on the corpse
        rb = GetComponent<Rigidbody>();
    }

    // Implement the Interact method to start dragging the corpse
    public void Interact()
    {
        StartDrag();
    }

    // Implement the GetHoverText method to show interaction message
    public string GetHoverText()
    {
        if (!rb.isKinematic)
        {
            return "Drag corpse";
        }

        return "";
    }

    private void StartDrag()
    {
        if (!isBeingDragged)
        {
            isBeingDragged = true;
            GameState.PlayerIsDraggingCorpse = true; // Set dragging state
            Debug.Log("Dragging started");
        }
    }

    private void StopDrag()
    {
        isBeingDragged = false;
        GameState.PlayerIsDraggingCorpse = false; // Set dragging state
        Debug.Log("Dragging stopped");
    }

    private void Update()
    {
        // Handle stopping drag when the interaction key is released while dragging
        if (isBeingDragged && Input.GetButtonUp("Interact"))
        {
            StopDrag();
        }
    }

    private void FixedUpdate()
    {
        // If dragging, move the corpse towards the player's position
        if (isBeingDragged && playerController != null)
        {
            // Calculate the target position relative to the player
            Vector3 followPosition = playerController.transform.position + playerController.transform.forward * 1f;
            followPosition.y = transform.position.y; // Keep the Y position constant to allow gravity to work naturally

            // Apply a velocity towards the target position
            Vector3 direction = (followPosition - transform.position);
            rb.velocity = direction; // Move the corpse towards the player
        }
    }
}
