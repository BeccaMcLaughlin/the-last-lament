using System;

public static class GameState
{
    private static int _processedCorpses;

    public static bool hasActiveCorpse = false;
    public static bool PlayerIsDraggingCorpse = false;

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

    public static int Corruption = 1;
}
