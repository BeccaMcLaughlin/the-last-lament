using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    void Start()
    {
        UpdateSlotHighlight();
    }

    void Update()
    {
        // Detect mouse scroll and move slot up/down
        // This might need a refactor for the name because it's not really a scroll input on console
        HandleScrollInput();

        // Detect pressing q to drop an item
    }

    public bool PickupItem(ScriptedItem item)
    {
        // Add current pick up item at the positon in the inventory if it's empty
        InventoryItem selectedInventoryItem = inventoryItems[GameState.SelectedInventoryPanel];
        if (!selectedInventoryItem.IsOccupied)
        {
            selectedInventoryItem.AssignItem(item);
            Debug.Log($"Picked up {item.ItemName} and placed it in slot {GameState.SelectedInventoryPanel}");
            return true;
        }

        return false;
    }

    private void HandleScrollInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            // Clamp from 0 to 4 based on direction of scrollwheel
            int newSelectedSlot = GameState.SelectedInventoryPanel += Input.GetAxis("Mouse ScrollWheel") > 0f ? 1 : -1;
            GameState.SelectedInventoryPanel = Mathf.Clamp(newSelectedSlot, 0, inventoryItems.Count - 1);
            UpdateSlotHighlight();
        }
    }

    private void UpdateSlotHighlight()
    {
        // Update each inventory item based on the selected panel in GameState
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].SetPanel(i == GameState.SelectedInventoryPanel);
        }
    }
}
