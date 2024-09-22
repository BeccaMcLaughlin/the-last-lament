using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class InteractRebindButton : MonoBehaviour
{
    public InputActionReference actionReference;
    public Button rebindButton;
    public Text bindingDisplayNameText;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    void Start()
    {
        UpdateBindingDisplay();
        rebindButton.onClick.AddListener(() => StartRebinding());
    }

    void UpdateBindingDisplay()
    {
        if (actionReference != null && actionReference.action != null)
        {
            var bindingIndex = actionReference.action.GetBindingIndexForControl(actionReference.action.controls[0]);
            string displayString = actionReference.action.GetBindingDisplayString(bindingIndex);
            bindingDisplayNameText.text = displayString;
        }
    }

    public void StartRebinding()
    {
        if (rebindingOperation != null)
            rebindingOperation.Cancel();

        bindingDisplayNameText.text = "Press a key...";

        rebindingOperation = actionReference.action.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position") // Exclude mouse movement
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();
        rebindingOperation = null;

        UpdateBindingDisplay();

        // Save the rebinds
        SaveRebinds();
    }

    void SaveRebinds()
    {
        var rebinds = actionReference.action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(actionReference.action.name + "Rebinds", rebinds);
    }

    void LoadRebinds()
    {
        var rebinds = PlayerPrefs.GetString(actionReference.action.name + "Rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
            actionReference.action.LoadBindingOverridesFromJson(rebinds);
    }
}
