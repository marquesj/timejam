using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class TakeDamageOnTouch : MonoBehaviour
{
    public LayerMask layermask;
    public float damage = 1;
    private Health health;
    private void Awake() 
    {
   
        health = GetComponent<Health>();
    }
   
    private void OnTriggerEnter2D(Collider2D other) 
    {
      //  Debug.Log("hgeuh");
        
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            health.Damage(damage);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
    //    Debug.Log("hgeuh");
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            health.Damage(damage);
        }        
    }
}
