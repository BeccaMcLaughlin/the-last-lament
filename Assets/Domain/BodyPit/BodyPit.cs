using TMPro;
using UnityEngine;

public class BodyPit : MonoBehaviour, IInteractable
{
    public GameObject corpsePrefab; // Prefab of the corpse to spawn
    public Transform spawnPoint; // Location where the corpse will spawn

    // Implement the Interact method from IInteractable
    public void Interact()
    {
        // Spawn the corpse at the specified spawn point
        if (!GameState.hasActiveCorpse)
        {
            Instantiate(corpsePrefab, spawnPoint.position, corpsePrefab.transform.rotation);
            GameState.hasActiveCorpse = true;
            Debug.Log("A corpse has been retrieved from the pile.");

            // TODO: Move this to when the ritual is complete and the body is in the alcove
            GameState.ProcessedCorpses += 1;

            // Increase the corruption meter randomly
            GameState.Corruption += UnityEngine.Random.Range(0, 2);
        }
    }

    // Implement the GetHoverText method to show the interaction message
    public string GetHoverText()
    {
        if (GameState.ProcessedCorpses == GameState.TotalCorpses)
        {
            return "No more corpses to process today";
        }

        return GameState.hasActiveCorpse ? "You must complete the current ritual before taking a fresh corpse from the pile" : "Take corpse from pile";
    }
}