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
    public AudioSource shootdownSound;
    public AudioSource wallSlideSound;
    //#################################################
    private CharacterControl characterControl;
    private ShootController shootController;
   private AudioSource[] audioSources;
   private bool running = false;
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
        characterControl.WallJumpEvent += JumpEvent;
        characterControl.checkGround.landedEvent += LandEvent;
        characterControl.checkWall.walledEvent += WallSlideEvent;
        characterControl.SlideEvent += SlideEvent;
        characterControl.BounceEvent += BounceEvent;

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

        foreach(AudioSource audio in audioSources)
        {
           audio.Stop();
        }
    }
    private void Update() {
       if((!characterControl.checkGround.grounded || characterControl.sliding) && walkSound.isPlaying)
         walkSound.Stop();
      if(characterControl.checkGround.grounded && !characterControl.sliding && running &&  !walkSound.isPlaying)
         walkSound.Play();
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
      wallSlideSound.Play();
    }
    private void SlideEvent() 
    {
       slideSound.Play();
    }
    private void StartWalkingEvent() 
    {
       running=true;
       walkSound.Play();
    }
    private void StopWalkingEvent() 
    {
       running=false;
       walkSound.Stop();
    }
    private void BounceEvent()
    {
      shootdownSound.Play();
    }
}