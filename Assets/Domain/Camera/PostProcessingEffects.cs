using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffects : MonoBehaviour
{
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;

    private float shiftSpeedMultiplier = 0.05f; // Controls how fast the shift occurs
    private float maxShiftRange = 0.2f; // Maximum range for x and y shifts

    void Start()
    {
        PostProcessVolume postProcessingVolume = GetComponent<PostProcessVolume>();
        chromaticAberration = postProcessingVolume.profile.GetSetting<ChromaticAberration>();
        lensDistortion = postProcessingVolume.profile.GetSetting<LensDistortion>();
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
}
