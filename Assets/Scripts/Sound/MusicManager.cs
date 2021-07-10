using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public TimeEvents timeEvents; 
    public AudioSource normalSong;
    public AudioSource slowSong;
    public AudioSource comebackSound;
    public bool overridePreviousPlayer = false;
    void Start()
    {

        GameObject[] previousPlayers = GameObject.FindGameObjectsWithTag("SoundManager");

        Debug.Log(previousPlayers.Length);
        GameObject previousPlayer = null;
        foreach(GameObject p in previousPlayers)
        {
            if(p!=gameObject)
                previousPlayer = p;

        }

        if(previousPlayer!=null && overridePreviousPlayer)
            Destroy(previousPlayer);
        else if(previousPlayer != null)
        {
            Destroy(gameObject);
            return;
        }
            

        if(slowSong !=null)
        {
            slowSong.volume = 0;
            slowSong.pitch= 0.444f;

        }

        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable() 
    {
        timeEvents.SlowTimeEvent += SlowDownTime;
        timeEvents.RestoreTimeEvent += RestoreTime;
        timeEvents.StopTimeEvent += StopTime;
        timeEvents.ContinueTimeEvent += ContinueTime;
    }    
    private void OnDisable() 
    {
        timeEvents.SlowTimeEvent -= SlowDownTime;
        timeEvents.RestoreTimeEvent -= RestoreTime;
        timeEvents.StopTimeEvent -= StopTime;
        timeEvents.ContinueTimeEvent -= ContinueTime;    
    }

    private void SlowDownTime()
    {
        slowSong.volume = 1;
        slowSong.pitch = 1;
        normalSong.pitch = 2.25f;
        normalSong.volume = 0;
    }
    private void RestoreTime()
    {
        slowSong.volume = 0;
        normalSong.volume = 1;  
        slowSong.pitch = 0.444f;
        normalSong.pitch = 1;  
    }
    private void StopTime()
    {
        slowSong.Pause();
        normalSong.Pause();
    }
    private void ContinueTime()
    {
        slowSong.Play();
        normalSong.Play();

        comebackSound.Play();
    }
}
