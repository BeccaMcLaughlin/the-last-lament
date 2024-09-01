using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private Image panel;
    private Image itemImage;
    private ScriptedItem item;
    private FPSController player;
    public bool IsOccupied => item != null;

    void Start()
    {
        panel = GetComponent<Image>();
        itemImage = transform.Find("Image").GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (Input.GetButton("Drop Item"))
        {
            DropItem();
        }
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

    private void ClearItem()
    {
        item = null;
        itemImage.sprite = null; // Clear the child image
        itemImage.enabled = false; // Hide the item image
    }

    public void DropItem()
    {
        if (IsOccupied)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
            Vector3 dropPosition = player.transform.position + player.transform.forward;
            Instantiate(item.ItemSpawnObject, dropPosition, player.transform.rotation);
            ClearItem();
        }
    }
}