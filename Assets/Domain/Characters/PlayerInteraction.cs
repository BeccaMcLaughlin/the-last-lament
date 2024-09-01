using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 1f; // Maximum distance for interaction
    public TextMeshProUGUI interactionText; // UI Text element for displaying hover text

    private IInteractable currentInteractable; // Reference to the current interactable object
    private Camera playerCamera; // Reference to the player's camera

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        CheckForInteractable();

        // Check if the player presses the interaction key
        if (Input.GetButtonDown("Interact") && currentInteractable != null)
        {
            // Trigger the interaction
            currentInteractable.Interact();
        }
    }

    // Check if the player is looking at an interactable object
    private void CheckForInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // Ray from the center of the screen (crosshair)
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Check if the hit object has an IInteractable component
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                ShowInteractionText(interactable.GetHoverText());
                return;
            }
        }

        // If no interactable is found, clear the current reference and hide text
        currentInteractable = null;
        HideInteractionText();
    }

    // Show interaction text on the screen
    private void ShowInteractionText(string text)
    {
        interactionText.text = text;
    }

    // Hide interaction text
    private void HideInteractionText()
    {
        interactionText.text = "";
    }
}
