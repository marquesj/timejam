using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public TimeEvents timeEvents;
    [Header("Sounds")]
    public AudioSource jumpSound;
    public AudioSource landSound;
    public AudioSource slideSound;
    public AudioSource walkSound;
    public AudioSource shootSound;
    //#################################################
    private CharacterControl characterControl;
    private ShootController shootController;
   private AudioSource[] audioSources;

    void Awake()
    {
        characterControl = transform.parent.GetComponent<CharacterControl>();
        shootController = transform.parent.GetComponent<ShootController>();
        audioSources = GetComponents<AudioSource>();
    }
    void Start()
    {
        characterControl.StopRunningEvent += StopWalkingEvent;
        characterControl.StartRunningEvent += StartWalkingEvent;
        shootController.ShootEvent += ShootEvent;
        characterControl.JumpEvent += JumpEvent;
        characterControl.checkGround.landedEvent += LandEvent;
        characterControl.checkWall.walledEvent += WallSlideEvent;
        characterControl.SlideEvent += SlideEvent;

        timeEvents.StopTimeEvent += StopTime;
        timeEvents.ContinueTimeEvent += ContinueTime;
        timeEvents.SlowTimeEvent += SlowTime;
        timeEvents.RestoreTimeEvent += RestoreTime;
    }
    private void OnDestroy() {
        timeEvents.StopTimeEvent -= StopTime;
        timeEvents.ContinueTimeEvent -= ContinueTime;
        timeEvents.SlowTimeEvent -= SlowTime;
        timeEvents.RestoreTimeEvent -= RestoreTime;
    }
    private void StopTime()
    {
        walkSound.Stop();
    }
    private void SlowTime()
    {
       foreach(AudioSource audioSource in audioSources)
         audioSource.pitch = 0.5f;
    }
    private void RestoreTime()
    {
       foreach(AudioSource audioSource in audioSources)
         audioSource.pitch = 2f;
    }
    private void ContinueTime()
    {

    }
    private void JumpEvent()
    {
        jumpSound.Play();
    }

    private void DoubleJumpEvent()
    {
       // jumpSound.Play();
    }
    private void WallJumpEvent()
    {
      // jumpSound.Play();
    }

    private void ShootEvent() 
    {
       shootSound.Play();
    }
    private void LandEvent() 
    {
       landSound.Play();
    }
    private void WallSlideEvent(bool aux) 
    {
      // slideSound.Play();
    }
    private void SlideEvent() 
    {
       slideSound.Play();
    }
    private void StartWalkingEvent() 
    {
       walkSound.Play();
    }
    private void StopWalkingEvent() 
    {
       walkSound.Stop();
    }
}