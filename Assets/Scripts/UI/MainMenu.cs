using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playFirstButton, loadFirstButton, controlsFirstButton;

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
    /*
    public void GotoMain() {
        eventSystem.currentSelected(null);
        eventSystem.currentSelected(playFirstButton);
    }

    public void GotoLoad() {
        EventSystem.current.firstSelectedGameObject(null);
        EventSystem.current.firstSelectedGameObject(loadFirstButton);
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
