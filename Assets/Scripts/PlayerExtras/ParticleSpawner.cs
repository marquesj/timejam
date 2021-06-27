using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{

    public CheckGround checkGround;
    public ParticleSystem LandParticles;
    public ParticleSystem walkParticles;
    public ParticleSystem smokeTrail;

    public float WalkParticlesChance = 0.1f;
    

    private bool isWalking = false;

    private void Update() {
        if(checkGround.grounded&&isWalking)
        {
     
            WalkUpdate();

        }
    }
    private void WalkUpdate()
    {
        float rng = Random.Range(0f,1f);
        if(rng < WalkParticlesChance)
            walkParticles.Play();
    }
    private void LandEvent() 
    {
       LandParticles.Play();
    }

    private void StartWalkingEvent()
    {
        isWalking = true;
    }
    private void StopWalkingEvent()
    {
        isWalking = false;
    }
    private void DoubleJumpEvent()
    {
        LandParticles.Play();
        smokeTrail.Play();
    }
    private void FlipEvent(float dir)
    {
        if(dir<0)
        {
            transform.rotation = Quaternion.Euler(0,180,0);

        }
        else
        {

            transform.rotation = Quaternion.Euler(0,0,0);
        }

    }
}
