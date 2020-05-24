using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _uiInstructions;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                _uiInstructions.SetActive(true);
                _pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                GamePaused = false;
            }
            else
            {
                _uiInstructions.SetActive(false);
                _pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                GamePaused = true;
            }
        }
    }

    public void ContinueClick()
    {
        _uiInstructions.SetActive(true);
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void QuitGame()
    {
        _uiInstructions.SetActive(true);
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        SceneManager.LoadScene(0);
    }
}
