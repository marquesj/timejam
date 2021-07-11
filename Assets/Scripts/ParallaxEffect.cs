using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ParallaxEffect : MonoBehaviour
{
    public float length, startpos;
    public float parallaxFactor = 1;
    public GameObject cam;
    public float PixelsPerUnit;

    float temp;
    float distance;
 
    void Start()
    {
        Debug.Log("test");
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    


 
    void FixedUpdate()
    {
        temp     = cam.transform.position.x * (1 - parallaxFactor);
        distance = cam.transform.position.x * parallaxFactor;
    
        Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);
       
        transform.position =  PixelPerfectClamp(newPosition, PixelsPerUnit);
    
        if (temp > startpos + (length / 2))    startpos += length;
        else if (temp < startpos - (length / 2)) startpos -= length;
    } 

    private Vector3 PixelPerfectClamp(Vector3 locationVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), Mathf.CeilToInt(locationVector.y * pixelsPerUnit), Mathf.CeilToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
}