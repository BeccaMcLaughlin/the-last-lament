using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private Image panel;
    private Image itemImage;
    private ScriptedItem item;
    public bool IsOccupied => item != null;

    private void Start()
    {
        panel = GetComponent<Image>();
        itemImage = transform.Find("Image").GetComponentInChildren<Image>();
    }

    public void SetPanel(bool isHighlighted)
    {
        Color panelColor = panel.color;
        panelColor.a = isHighlighted ? 1.0f : 0.1f; // Fully visible if highlighted, otherwise transparent
        panel.color = panelColor;
    }

    // Assign an item to this inventory slot
    public void AssignItem(ScriptedItem newItem)
    {
        item = newItem;
        itemImage.sprite = newItem.ItemIcon;
        itemImage.enabled = true; // Enable the icon image
    }
}