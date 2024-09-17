using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffects : MonoBehaviour
{
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private DepthOfField depthOfField;

    private float shiftSpeedMultiplier = 0.05f; // Controls how fast the shift occurs
    private float maxShiftRange = 0.2f; // Maximum range for x and y shifts

    private void OnEnable()
    {
        PlayerSanity.ShowGameOverScreen += AddDepthOfFieldBlur;
    }

    private void OnDisable()
    {
        PlayerSanity.ShowGameOverScreen -= AddDepthOfFieldBlur;
    }

    void Start()
    {
        PostProcessVolume postProcessingVolume = GetComponent<PostProcessVolume>();
        chromaticAberration = postProcessingVolume.profile.GetSetting<ChromaticAberration>();
        lensDistortion = postProcessingVolume.profile.GetSetting<LensDistortion>();
        depthOfField = postProcessingVolume.profile.GetSetting<DepthOfField>();

        if (depthOfField != null)
        {
            depthOfField.active = false;
        }
    }

    void Update()
    {
        if (lensDistortion != null && lensDistortion.intensity.value > 0f)
        {
            // Continuously shift x and y values based on current intensity
            ShiftLensDistortionCenter();
        }
    }

    public void UpdateIntensityOfSettings(float intensityModifier)
    {
        chromaticAberration.intensity.value = intensityModifier;
        lensDistortion.intensity.value = intensityModifier * 30f;
    }

    private void ShiftLensDistortionCenter()
    {
        // Calculate the shift rate based on the current intensity of lens distortion
        float shiftSpeed = lensDistortion.intensity.value * shiftSpeedMultiplier;

        // Use sine and cosine functions to create smooth oscillation effects for x and y (Maffs)
        float xShift = Mathf.Sin(Time.time * shiftSpeed) * maxShiftRange;
        float yShift = Mathf.Cos(Time.time * shiftSpeed) * maxShiftRange;

        // Set the center x and y values to create a shifting effect
        lensDistortion.centerX.value = xShift;
        lensDistortion.centerY.value = yShift;
    }

    private void AddDepthOfFieldBlur()
    {
        if (depthOfField != null)
        {
            depthOfField.active = true;
        }
    }
}
