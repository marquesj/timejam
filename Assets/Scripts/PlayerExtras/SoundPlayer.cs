using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Header("Sounds")]
    public AudioSource jumpSound;
    public AudioSource landSound;
    public AudioSource slideSound;
    public AudioSource walkSound;
    //#################################################
    private CharacterControl characterControl;
    private ShootController shootController;


    void Awake()
    {
        characterControl = transform.parent.GetComponent<CharacterControl>();
        shootController = transform.parent.GetComponent<ShootController>();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void JumpEvent()
    {
       // jumpSound.Play();
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
       
    }
    private void LandEvent() 
    {
       
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