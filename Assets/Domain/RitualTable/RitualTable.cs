using System.Collections;
using UnityEngine;

public class RitualTable : MonoBehaviour, IInteractable
{
    public Transform spawnPoint; // The point on the table where the corpse should be placed
    private Collider detectedCorpse;

    private bool HasCorpseOnTable = false;

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
        if (GameState.HasCompletedActiveRitual)
        {
            return "Lay the corpse in an alcove before performing another cleansing";
        } else if (!HasCorpseOnTable)
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
            HasCorpseOnTable = true;
            GameState.HasCompletedActiveRitual = false;

            // Disable corpse physics
            // TODO: Is this the right place for this logic?
            detectedCorpse.attachedRigidbody.isKinematic = true;

            // TODO: Move this from a timer over to actual logic when all items are done
            StartCoroutine(CanRemoveCorpse());
        }
    }

    private IEnumerator CanRemoveCorpse()
    {
        yield return new WaitForSeconds(4f); // In reality this would be the individual items all collected for the ritual

        HasCorpseOnTable = false;
        GameState.HasCompletedActiveRitual = true;
        detectedCorpse.attachedRigidbody.isKinematic = false;
    }
}