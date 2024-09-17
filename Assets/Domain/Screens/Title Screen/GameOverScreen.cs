using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    GameObject gameOverScreen;

    private void Start()
    {
        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverUI");
        gameOverScreen.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerSanity.ShowGameOverScreen += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        PlayerSanity.ShowGameOverScreen -= ShowGameOverScreen;
    }

    public void ShowTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    private void ShowGameOverScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        Debug.Log("Oops");
    }
}
