using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button playFirstButton, loadFirstButton, controlsFirstButton;

    public GameObject mainScreen, controlScreen, loadScreen;

    public EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }
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
    
    public void GotoMain() {
        //if (EventSystem.current.currentSelectedGameObject == null)
            playFirstButton.Select();
    }

    public void GotoLoad() {
        //if (EventSystem.current.currentSelectedGameObject == null)
            loadFirstButton.Select();
    }

    public void GotoControls()
    {
        //if (EventSystem.current.currentSelectedGameObject == null)
            controlsFirstButton.Select();
    }

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
