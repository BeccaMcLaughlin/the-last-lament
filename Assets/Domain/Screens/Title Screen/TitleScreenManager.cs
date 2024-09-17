using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Called when the Start Game button is pressed
    public void StartGame()
    {
        // Load the main game scene asynchronously to allow for a loading screen
        SceneManager.LoadSceneAsync("MainGameScene");
    }

    // Called when the Options button is pressed
    public void ShowOptions()
    {
        // Activate the options menu or load an options scene
        // You could use SetActive() for a UI panel or load another scene
    }

    // Called when the Quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }
}