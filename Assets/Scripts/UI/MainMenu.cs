using System.Diagnostics;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playFirstButton, loadFirstButton, controlsFirstButton;
    //public Leaderboard leaderboard;
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
    }
    /*public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToLeaderboard() {
        SceneManager.LoadScene("HighScore");
    }*/
    /*
    public void GotoMain() {
        UnityEngine.EventSystem.current.currentSelectedGameObject (null);
        UnityEngine.EventSystem.current.currentSelectedGameObject (playFirstButton);
    }

    public void GotoLoad() {
        UnityEngine.EventSystem.current.currentSelectedGameObject (null);
        UnityEngine.EventSystem.current.currentSelectedGameObject (loadFirstButton);
    }
    */


    public void QuitGame () {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
        UnityEngine.Debug.Log("Quit");
        Application.Quit();
    }

    public void SetName(string name)
    {
        //leaderboard.currentName = name;
        //SceneManager.LoadScene("lvl 1");
    }
}
