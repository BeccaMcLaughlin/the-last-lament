using TMPro;
using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [TextArea(3, 10)]
    public string noteText;
    private TextMeshProUGUI textMeshInstance;

    void Start()
    {
        // Get the Reading UI reference from the UIManager Singleton
        if (UIManager.Instance != null && UIManager.Instance.notesUI != null)
        {
            textMeshInstance = UIManager.Instance.notesUI.transform.Find("NoteText").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Reading UI not found. Ensure UIManager is in the scene and readingUI is assigned.");
        }
    }

    public string GetHoverText()
    {
        return "Read note";
    }

    public void Interact()
    {
        if (UIManager.Instance != null && UIManager.Instance.notesUI != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.notesUI.SetActive(true);
            textMeshInstance.text = noteText;
            Time.timeScale = 0f;
        }
    }
}
