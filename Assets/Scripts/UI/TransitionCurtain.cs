using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionCurtain : MonoBehaviour
{
    public float duration;
    public float offset;
    private float startX;
    public bool destroyMusicManager = false;
    // Start is called before the first frame update
    void Start()
    {
        startX = transform.localPosition.x;
        OpenCurtain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Open")]
    public void OpenCurtain()
    {
        if(duration>0)
            StartCoroutine(MoveTo(startX + offset, duration));
        else
            transform.position = new Vector3(startX + offset,transform.position.y, transform.position.z);
    }
    [ContextMenu("Close")]
    public void CloseCurtain()
    {
        if(duration>0)
            StartCoroutine(MoveTo(startX, duration));
        else
            transform.position = new Vector3(startX ,transform.position.y, transform.position.z);
        Invoke("NextRoom",duration+0.1f);
    }
    private void NextRoom()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex + 1);
    }
    private IEnumerator MoveTo(float targetX,float duration)
    {
        Time.timeScale = 0;
        targetX = transform.parent.position.x + targetX;
        float startTime = Time.realtimeSinceStartup;
        float startingX = transform.position.x;
        float percent = 0;
        float x;
        while(percent < 1)
        {
       
            percent = (Time.realtimeSinceStartup - startTime)/duration;
            x = Mathf.Lerp(startingX, targetX, percent);
            transform.position = new Vector3(x,transform.position.y, transform.position.z);
            yield return null;
        }
        Time.timeScale = 1;
    }
}
