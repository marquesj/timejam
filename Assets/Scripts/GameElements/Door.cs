using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    public BoxCollider2D door;
    public BoxCollider2D entry;
    private Animator animator;
    private AudioSource openSound;
    void Start()
    {
        animator = GetComponent<Animator>();
        openSound = GetComponent<AudioSource>();
        entry.enabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Open")]
    private void Open()
    {
        openSound.Play();
        animator.SetTrigger("Open");
        door.enabled = false;
        entry.enabled = true;
    }
    [ContextMenu("Close")]
    private void Close()
    {
        animator.Play("Closed");
        door.enabled = true;
        entry.enabled = false;
    }

    public void SetState(bool state)
    {
        if(state)
        {
            Open();
        }else
        {
            Close();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex + 1);
    }
}
