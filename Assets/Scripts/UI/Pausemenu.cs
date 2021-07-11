using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class Pausemenu : MonoBehaviour
{
    public static bool GameISPaused = false;
    public GameObject pauseMenuUI;

    public Button pauseFirstButton;

    public Button controlsFirstButton;

    // Update is called once per frame
    private void Start() {
        Cursor.visible = false;
    }
    public void Paused()
    {
        if (GameISPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameISPaused = false;
    }

    void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameISPaused = true;

        GotoResume();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
        UnityEngine.Debug.Log("Quit");
        Application.Quit();
    }


    public void GotoResume()
    {
        //if (EventSystem.current.currentSelectedGameObject == null)
        pauseFirstButton.Select();
    }

    public void GotoControls()
    {
        //if (EventSystem.current.currentSelectedGameObject == null)
        controlsFirstButton.Select();
    }

}
