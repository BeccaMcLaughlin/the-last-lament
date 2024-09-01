using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public ScriptedItem item;
    private Inventory inventory;

    public string GetHoverText()
    {
        return $"Pick up {item.ItemName}";
    }

    public void Interact()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        bool added = inventory.PickupItem(item);

        if (added)
        {
            // If the item was added successfully, destroy the pickup object
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Slot has an item already!");
        }
    }
}
