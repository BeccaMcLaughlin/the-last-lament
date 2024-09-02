using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualMatch : MonoBehaviour, IInteractable
{
    public ScriptedItem item;
    public int ritualIndex = 0;
    private Inventory inventory;
    public static event Action<int> OnHandleMatch;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }

    public string GetHoverText()
    {
        return $"Place {item.ItemName}";
    }

    public void Interact()
    {
        InventoryItem itemInPlayersInventory = inventory.inventoryItems[GameState.SelectedInventoryPanel];

        if (itemInPlayersInventory.item == item)
        {
            Debug.Log("Remove this item from both the ritual altar and inventory. update RitualTableItems.cs to show the item has been placed and that objective fulfilled.");
            HandleMatch(itemInPlayersInventory);
        }
        else
        {
            Debug.Log("Not a match!");
            inventory.FlashInventoryMessage("Find the matching item");
        }
    }

    private void HandleMatch(InventoryItem itemInPlayersInventory)
    {
        // Remove physics so item cannot be interacted with twice
        gameObject.GetComponent<Collider>().enabled = false;

        // Remove the item visually from the altar
        StartCoroutine(RemoveItemWithEffects());

        // Remove the item from the player's inventory
        itemInPlayersInventory.ClearItem();

        // Update inventory message and feedback
        inventory.FlashInventoryMessage("You add the item to the corpse");

        Debug.Log("Remove this item from both the ritual altar and inventory. Update RitualTableItems.cs to show the item has been placed and that the objective is fulfilled.");
    }

    private IEnumerator RemoveItemWithEffects()
    {
        // Trigger particles or visual effects (you can adjust this part to fit your effect needs)
        // Assuming you have a ParticleSystem component or effect setup
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }

        // Fade out the object over time
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                Color color = renderer.material.color;
                color.a = Mathf.Lerp(1, 0, t);
                renderer.material.color = color;
                yield return null;
            }
        }

        // Send event that this item has been removed to RitualTableItems
        OnHandleMatch?.Invoke(ritualIndex);

        // Finally, remove the object from the scene
        Destroy(gameObject);
    }
}
