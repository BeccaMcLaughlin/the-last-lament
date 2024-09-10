using System;
using UnityEngine;

public static class GameState
{
    private static int _processedCorpses;

    public static bool HasActiveCorpse = false;
    public static bool PlayerIsDraggingCorpse = false;
    public static bool HasCorpseOnTable = false;
    public static GameObject CorpseToPutInsideAlcove = null;

    public static event Action OnProcessedCorpsesChanged;

    public static int ProcessedCorpses
    {
        get => _processedCorpses;
        set
        {
            _processedCorpses = value;
            OnProcessedCorpsesChanged?.Invoke(); // Trigger the event
        }
    }

    public static int TotalCorpses = 10;

    public static float Corruption = 1f;

    public static int SelectedInventoryPanel = 0;

    public static bool playerIsCrouching = false;
}
