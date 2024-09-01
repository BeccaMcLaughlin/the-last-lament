using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualMatch : MonoBehaviour, IInteractable
{
    public ScriptedItem item;
    private Inventory inventory;

    public string GetHoverText()
    {
        return $"Place {item.ItemName}";
    }

    public void Interact()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        InventoryItem itemInPlayersInventory = inventory.inventoryItems[GameState.SelectedInventoryPanel];

        if (itemInPlayersInventory.item == item)
        {
            Debug.Log("Add the item to the ritual altar and remove from inventory");
        }
        else
        {
            Debug.Log("Not a match!");
        }
    }
}
