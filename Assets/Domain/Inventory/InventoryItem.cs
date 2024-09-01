using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private Image panel;
    private ScriptedItem item;

    private void Start()
    {
        panel = GetComponent<Image>();
    }

    public void SetPanel(bool isHighlighted)
    {
        Color panelColor = panel.color;
        panelColor.a = isHighlighted ? 1.0f : 0.1f; // Fully visible if highlighted, otherwise transparent
        panel.color = panelColor;
    }

    public void AssignItem(ScriptedItem newItem)
    {
        // Nothing here yet
    }
}