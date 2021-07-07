using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;
public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Light2D light2D;
    
    private void Awake() {
        light2D = GetComponent<Light2D>();
        if(light2D!=null)
            light2D.intensity = 0;
    }
    public void Shoot()
    {
        Instantiate(projectilePrefab,transform.position,transform.rotation);
    }
    public void Shoot(float timeOffset)
    {
        GameObject obj = Instantiate(projectilePrefab,transform.position,transform.rotation);
        TimedElement timedElement = obj.GetComponent<TimedElement>();
        timedElement.SetTimeOffset(timeOffset);

        if(light2D !=null)
            StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {   
        light2D.intensity = 1;
        yield return new WaitForSeconds(0.1f);
        light2D.intensity = 0;
    }
}
