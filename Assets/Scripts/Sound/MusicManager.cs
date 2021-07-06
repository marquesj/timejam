using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public TimeEvents timeEvents; 
    public AudioSource normalSong;
    public AudioSource slowSong;
    void Start()
    {
        timeEvents.SlowTimeEvent += SlowDownTime;
        timeEvents.RestoreTimeEvent += RestoreTime;
        timeEvents.StopTimeEvent += StopTime;
        timeEvents.ContinueTimeEvent += ContinueTime;

        slowSong.volume = 0;
    }


    private void SlowDownTime()
    {
        slowSong.volume = 1;
        normalSong.volume = 0;
    }
    private void RestoreTime()
    {
        slowSong.volume = 0;
        normalSong.volume = 1;    
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
    }
}
