using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcove : MonoBehaviour, IInteractable
{
    public Transform spawnPoint;
    public GameObject corpse;
    public string GetHoverText()
    {
        return corpse ? "This alcove already has a corpse in it" : "Put completed corpse into alcove";
    }

    public void Interact()
    {
        if (GameState.CorpseToPutInsideAlcove != null && !corpse)
        {
            corpse = GameState.CorpseToPutInsideAlcove.gameObject;
            corpse.transform.position = spawnPoint.position;
            corpse.transform.rotation = spawnPoint.rotation;
            GameState.HasCorpseOnTable = false;
            GameState.HasActiveCorpse = false;
            GameState.CorpseToPutInsideAlcove = null;
        }
    }
}
