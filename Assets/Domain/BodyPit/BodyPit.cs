using UnityEngine;

public class BodyPit : MonoBehaviour, IInteractable
{
    public GameObject corpsePrefab; // Prefab of the corpse to spawn
    public Transform spawnPoint; // Location where the corpse will spawn

    // Implement the Interact method from IInteractable
    public void Interact()
    {
        // Spawn the corpse at the specified spawn point
        Instantiate(corpsePrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("A corpse has been retrieved from the pile.");
    }

    // Implement the GetHoverText method to show the interaction message
    public string GetHoverText()
    {
        return "Take corpse from pile";
    }
}