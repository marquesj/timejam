using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyElement : MonoBehaviour
{
    public float bounciness = 0;
    public float bouncinessDelay = 0;
    private float actualBounciness;
    private void Awake() {
        if(bouncinessDelay > 0)
        {
            actualBounciness = bounciness;
            bounciness = 0;
            Invoke("SetBounciness", bouncinessDelay);
        }
    }
    private void SetBounciness()
    {
        bounciness = actualBounciness;        
    }
}
