using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void GameQuit()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

    public void StartGame()
    {
        Debug.Log("Loading the game level...");
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
