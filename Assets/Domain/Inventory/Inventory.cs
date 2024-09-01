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

    public void PickupItem()
    {
        // Add current pick up item at the positon in the inventory if it's empty

        // Then destroy it from the game world because it should be stored in a new instance of InventoryItem
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
