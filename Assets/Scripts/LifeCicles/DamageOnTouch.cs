using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public LayerMask layermask;
    public float damage = 1;
    private void OnTriggerEnter(Collider other) {
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<Health>().Damage(damage);
        }
    }
    private void OnCollisionEnter(Collision other) 
    {
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<Health>().Damage(damage);
        }        
    }
}
