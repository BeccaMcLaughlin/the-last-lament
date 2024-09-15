using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject notesUI;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CloseNotesUI()
    {
        if (notesUI != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            notesUI.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogWarning("Notes UI is not assigned or found.");
        }
    }
}
