using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class TakeDamageOnTouch : MonoBehaviour
{
    public LayerMask layermask;
    public float damage = 1;
    public List<string> imuneTags;
    private Health health;
    public AudioSource audioSource;
    private void Awake() 
    {
   
        health = GetComponent<Health>();
    }
   
    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        foreach(string t in imuneTags)
        {
            if(other.gameObject.tag == t) {
                return;
            }
                
        }
        
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            Debug.Log("hgeuh");
            health.Damage(damage);
            if(audioSource!=null)
                audioSource.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
    
        foreach(string t in imuneTags)
        {
            if(other.gameObject.tag == t)
                return;
        }
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            health.Damage(damage);
            if(audioSource!=null)
                audioSource.Play();
        }        
    }
}
