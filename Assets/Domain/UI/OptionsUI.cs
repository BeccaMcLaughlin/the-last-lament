using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsUI : MonoBehaviour
{
    public Slider sensitivitySlider;

    void Start()
    {
        sensitivitySlider.value = PlayerActions.Instance.MouseSensitivity;
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    void OnSensitivityChanged(float value)
    {
        PlayerActions.Instance.MouseSensitivity = value;
    }
    
    public void GoToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}