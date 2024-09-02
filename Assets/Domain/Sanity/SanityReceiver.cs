using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityReceiver : MonoBehaviour
{
    public float maximumSanity = 100f;
    public float sanityModifier = 0f;

    public float currentSanity = 100f;

    public static event Action OnSanityReachesZero;
    public static event Action OnSanityReachesMaximum;

    // Update is called once per frame
    void Update()
    {
        if (sanityModifier != 0f)
        {
            // Update by the sanityModifier per second using time.delatTime
            currentSanity += sanityModifier * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, 0f, maximumSanity);
        }

        if (currentSanity == 0)
        {
            // If reaches 0, emit event to the script that will handle the logic for this entity
            OnSanityReachesZero?.Invoke();
        } else if (currentSanity == maximumSanity)
        {
            // If reaches the maximum value, emit event to the script that will handle the logic for this entity
            OnSanityReachesMaximum?.Invoke();
        }
    }
}
