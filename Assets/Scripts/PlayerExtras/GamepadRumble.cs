using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GamepadRumble : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask mask;
    public float maxIntensity =1;
    public float rumbleTime = 0.5f;

    private void Start() {
        Gamepad.current.ResetHaptics();
    }
    private IEnumerator PlayHaptics(float leftMotorIntensityPercent,float rightMotorIntensityPercent)
    {
        Gamepad.current.SetMotorSpeeds(maxIntensity*leftMotorIntensityPercent,maxIntensity*rightMotorIntensityPercent);
        yield return new WaitForSeconds(rumbleTime);
        Gamepad.current.ResetHaptics();
    }

    public void StartHaptics()
    {
        StartCoroutine(PlayHaptics(1,1));
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(mask == (mask | (1 << other.gameObject.layer) ))
            StartCoroutine(PlayHaptics(1,1));
    }
    private void OnDestroy() {
        Gamepad.current.ResetHaptics();
    }
}
