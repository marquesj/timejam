using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem deathParticles;
    public ParticleSystem landParticles;
    public ParticleSystem slideParticles;
    public ParticleSystem walkParticles;
    public ParticleSystem automaticWalkParticles;
    public float walkParticleChance = 0.01f;
    private CharacterControl characterControl;
    private ShootController shootController;
    private Health health;
    private bool walking;
    void Awake()
    {
        characterControl = transform.parent.GetComponent<CharacterControl>();
        shootController = transform.parent.GetComponent<ShootController>();
        health = transform.parent.GetComponent<Health>();
    }

    // Update is called once per frame
    void Start()
    {
        characterControl.StopRunningEvent += StopWalkingEvent;
        characterControl.StartRunningEvent += StartWalkingEvent;
        characterControl.WallJumpEvent += WallJumpEvent;
       // shootController.ShootEvent += ShootEvent;
        characterControl.JumpEvent += JumpEvent;
        characterControl.checkGround.landedEvent += LandEvent;
        characterControl.SlideEvent += SlideEvent;
        characterControl.SlideEvent += SlideEvent;
        health.DeathEvent += DeathEvent;
        //characterControl.StopSlideEvent += StopSlideEvent;
        //characterControl.checkWall.walledEvent += ClingEvent;
    }

  

    private void FixedUpdate() {
        if(walking && characterControl.checkGround.grounded)
        {
            if(Random.Range(0f,1f) > walkParticleChance)
            {
                walkParticles.Play();
            }
        }
    }
    private void JumpEvent()
    {

    }
    private void ShootEvent()
    {

    }


    private void DoubleJumpEvent()
    {

    }
    private void WallJumpEvent()
    {
        automaticWalkParticles.Play();
    }
    private void LandEvent() 
    {
        landParticles.Play();
    }



    private void ClingEvent(bool aux)
    {

    }

    private void StartWalkingEvent()
    {
        walking = true;
    }
    private void StopWalkingEvent()
    {
        walking = false;
    }

    private void SlideEvent()
    {
        slideParticles.Play();
    }
    private void StopSlideEvent()
    {

    }
    private void DeathEvent()
    {
        deathParticles.Play();
    }
}
