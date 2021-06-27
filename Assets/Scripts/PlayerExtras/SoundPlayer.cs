using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource jumpSound;
    public AudioSource landSound;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void JumpEvent()
    {
        jumpSound.Play();
    }

    private void DoubleJumpEvent()
    {
        jumpSound.Play();
    }
    private void WallJumpEvent()
    {
        jumpSound.Play();
    }
    private void LandEvent() 
    {
       landSound.Play();
    }
}
