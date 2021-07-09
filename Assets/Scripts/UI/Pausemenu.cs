using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public static bool GameISPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    public void Paused() {
        if (GameISPaused) {
                Resume();
            }
            else {
                Pause();
            }
    }

    public void Resume () {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameISPaused = false;
    }

    void Pause () {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameISPaused = true;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit() {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
        UnityEngine.Debug.Log("Quit");
        Application.Quit();
    }
}
