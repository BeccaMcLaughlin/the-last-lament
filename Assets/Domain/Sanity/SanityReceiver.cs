using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityReceiver : MonoBehaviour
{
    public float maximumSanity = 100f;
    public float minimumSanity = 0f;
    public float sanityModifier = 0f;

    public float currentSanity = 100f;

    public static event Action OnSanityReachesZero;

    // Update is called once per frame
    void Update()
    {
        if (sanityModifier != 0f)
        {
            // Update by the sanityModifier per second using time.delatTime
            currentSanity += sanityModifier * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, minimumSanity, maximumSanity);
        }

        if (currentSanity == 0)
        {
            // If reaches 0, emit event to the script that will handle the logic for this entity
            // For example, a monster's sanity could be -100f to 0f and on 0f, the monster must go back to the darkness
            // Whereas a player's sanity could be 0f to 100f and on 0f, the player loses the game
            OnSanityReachesZero?.Invoke();
        }
    }
}
