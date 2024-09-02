using System.Collections;
using UnityEngine;

public class RitualTable : MonoBehaviour, IInteractable
{
    public Transform spawnPoint; // The point on the table where the corpse should be placed
    private Collider detectedCorpse;
    private RitualTableItems ritualTableItems = null;

    private void OnEnable()
    {
        RitualTableItems.CompleteRitual += CompleteRitual;
    }

    private void OnDisable()
    {
        RitualTableItems.CompleteRitual -= CompleteRitual;
    }

    private void Start()
    {
        ritualTableItems = GetComponent<RitualTableItems>();
    }

    private void Update()
    {
        detectedCorpse = FindNearbyCorpse();
    }

    // Find a nearby corpse within a 2-meter radius
    private Collider FindNearbyCorpse()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Corpse")) // Yum
            {
                return collider;
            }
        }
        return null; // No corpse found within range
    }

    public string GetHoverText()
    {
        if (GameState.CorpseToPutInsideAlcove != null)
        {
            return "Lay the corpse in an alcove before performing another cleansing";
        } else if (!GameState.HasCorpseOnTable)
        {
            return detectedCorpse ? "Put corpse on the table" : "No corpse nearby to place on the table";
        }
        else
        {
            return "Find items to complete the cleansing";
        }
    }

    public void Interact()
    {
        if (detectedCorpse != null)
        {
            // Move the detected corpse to the spawn point on the table
            detectedCorpse.transform.position = spawnPoint.position;
            detectedCorpse.transform.rotation = spawnPoint.rotation;
            GameState.PlayerIsDraggingCorpse = false; // Update game state to indicate the player is no longer dragging
            Debug.Log("Corpse placed on the table.");
            GameState.HasCorpseOnTable = true;
            GameState.CorpseToPutInsideAlcove = null;

            // Begin the game of spawning items to find
            if (ritualTableItems != null)
            {
                ritualTableItems.SelectItemsBasedOnCorruption();
            }

            // Disable corpse physics
            detectedCorpse.attachedRigidbody.isKinematic = true;
        }
    }

    private void CompleteRitual()
    {
        Debug.Log("Ritual complrte");

        GameState.CorpseToPutInsideAlcove = detectedCorpse.gameObject;
    }
}
