using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public LayerMask layermask;
    public float damage = 1;
    private void OnTriggerEnter2D(Collider2D other) {
//        UnityEngine.Debug.Log("akjajaja");
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {

            other.gameObject.GetComponent<Health>().Damage(damage);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
       // UnityEngine.Debug.Log("auuu");
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<Health>().Damage(damage);
        }        
    }
}
