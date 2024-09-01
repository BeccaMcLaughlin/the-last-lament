using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorpseCount : MonoBehaviour
{
    private TextMeshProUGUI corpseCountText; // Corpse counter to display at top right

    // Start is called before the first frame update
    void Start()
    {
        corpseCountText = GetComponent<TextMeshProUGUI>();
        UpdateUI();
    }

    private void OnEnable()
    {
        // Subscribe to GameState changes
        GameState.OnProcessedCorpsesChanged += UpdateUI;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameState.OnProcessedCorpsesChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        corpseCountText.text = $"Current corpses: {GameState.ProcessedCorpses} out of {GameState.TotalCorpses}";
    }
}
