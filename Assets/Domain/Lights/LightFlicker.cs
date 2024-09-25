using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    private Light lanternLight; // Reference to the Light component
    public float flickerFrequency = 0.01f; // Slower flicker frequency for a relaxing effect
    public float intensityVariation = 0.1f; // ±10% variation for a softer effect

    private float initialIntensity; // Store the initial intensity

    void Start()
    {
        // Get the Light component automatically and store the initial intensity
        lanternLight = GetComponent<Light>();
        initialIntensity = lanternLight.intensity;
    }

    void Update()
    {
        // Use sine to create a smooth flickering effect
        float timeCounter = Time.time * flickerFrequency;
        float flickerFactor = Mathf.Sin(timeCounter * Mathf.PI) * (0.5f * intensityVariation) + (1 - (0.5f * intensityVariation));

        // Set the intensity of the light with proper ±10% variation
        lanternLight.intensity = initialIntensity * flickerFactor;

        // Optional: Smooth color variation for a calming effect
        lanternLight.color = new Color(
            Random.Range(0.9f, 1f), // Red
            Random.Range(0.7f, 0.8f), // Green (narrower range for a softer look)
            Random.Range(0.4f, 0.5f) // Blue (narrower range)
        );
    }
}