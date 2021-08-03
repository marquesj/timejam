using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(entry!=null)
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
        if(animator!=null)
            animator.SetTrigger("Open");
        door.enabled = false;
        entry.enabled = true;
    }
    [ContextMenu("Close")]
    private void Close()
    {
        
        animator.SetTrigger("Close");
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
        if(other.gameObject.tag != "Player") {
            return;
        }

        Collider2D[] colliders = other.gameObject.GetComponents<Collider2D>();
        AudioSource[] audios = other.gameObject.GetComponents<AudioSource>();

        foreach(AudioSource audioSource in audios)
            audioSource.volume = 0;
        foreach(Collider2D col in colliders)
            col.enabled = false;

        GameObject.FindGameObjectWithTag("MainCamera").transform.Find("TransitionCurtain").GetComponent<TransitionCurtain>().CloseCurtain();
    }
}
