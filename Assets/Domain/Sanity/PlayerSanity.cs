using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerSanity : MonoBehaviour
{
    private SanityReceiver sanityReceiver;
    private Camera playerCamera;
    private PostProcessVolume postProcessingVolume;

    private float maxWarpIntensity = 1f; // Maximum warp intensity when sanity is at its lowest
    private float minWarpIntensity = 0f; // Minimum warp intensity when sanity is full

    // TODO: Split this logic out to the camera post processing script and use 
    // cameraScript = camera.postProcessingScript
    // and if no post processing script then just dont call camera.postProcessingScript.updateBasedOnIntensity(float intensity)
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;

    void Start()
    {
        // Get the SanityReceiver component from the player
        sanityReceiver = GetComponent<SanityReceiver>();
        playerCamera = GetComponentInChildren<Camera>();

        if (sanityReceiver == null || playerCamera == null)
        {
            Debug.LogError("SanityReceiver or camera component is missing on the player.");
        }

        postProcessingVolume = playerCamera.GetComponent<PostProcessVolume>();

        if (postProcessingVolume == null)
        {
            Debug.LogError("Need post processing volume on the camera");
        }

        chromaticAberration = postProcessingVolume.profile.GetSetting<ChromaticAberration>();
        lensDistortion = postProcessingVolume.profile.GetSetting<LensDistortion>();
        if (chromaticAberration == null || lensDistortion == null)
        {
            Debug.LogError("Need chromatic abberation or lens distortion on the camera");
        }

    }

    void Update()
    {
        if (sanityReceiver != null && sanityReceiver.sanityModifier != 0f)
        {
            // Calculate normalized sanity value (0 = no sanity, 1 = full sanity)
            float normalizedSanity = sanityReceiver.currentSanity / sanityReceiver.maximumSanity;

            // Calculate the warp intensity inversely proportional to sanity
            float warpIntensity = Mathf.Lerp(maxWarpIntensity, minWarpIntensity, normalizedSanity);

            // Update the camera warp effect based on the calculated intensity
            UpdateCameraWarpEffect(warpIntensity);
        }
    }

    private void UpdateCameraWarpEffect(float intensity)
    {
        // Example implementation for updating the camera warp effect based on intensity
        Debug.Log($"Updating camera warp effect with intensity: {intensity}");

        // Example: Apply the intensity to a hypothetical post-processing volume or shader effect
         postProcessingVolume.profile.GetSetting<ChromaticAberration>().intensity.value = intensity;
         postProcessingVolume.profile.GetSetting<LensDistortion>().intensity.value = intensity * 30f;
    }
}
