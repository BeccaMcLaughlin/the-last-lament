using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Called when the Start Game button is pressed
    public void StartGame()
    {
        Time.timeScale = 1f;

        Debug.Log(GameState.mainGameSceneBuildIndex);

        if (GameState.mainGameSceneBuildIndex == -1)
        {
            SceneManager.LoadSceneAsync("MainGameScene");
        }
        SceneManager.LoadSceneAsync(GameState.mainGameSceneBuildIndex);
    }

    // Called when the Options button is pressed
    public void ShowOptions()
    {
        // Activate the options menu or load an options scene
        // You could use SetActive() for a UI panel or load another scene
        SceneManager.LoadScene("OptionsScreen");
    }

    // Called when the Quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }
}