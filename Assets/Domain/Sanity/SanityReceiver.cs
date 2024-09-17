using UnityEngine;

public class SanityReceiver : MonoBehaviour
{
    public float maximumSanity = 100f;
    public float minimumSanity = 0f;
    public float sanityModifier = 0f;

    public float currentSanity = 100f;

    // Update is called once per frame
    void Update()
    {
        if (sanityModifier != 0f)
        {
            // Update by the sanityModifier per second using time.delatTime
            currentSanity += sanityModifier * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, minimumSanity, maximumSanity);
        }
    }

    public bool isAtMinimumSanity
    {
        get
        {
            return currentSanity <= minimumSanity;
        }
    }
}
