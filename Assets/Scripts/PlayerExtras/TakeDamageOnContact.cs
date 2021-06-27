using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageOnContact : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask weaknesses;
    private void OnCollisionEnter2D(Collision2D other) {
        if(weaknesses == (weaknesses | (1 << other.gameObject.layer)))
        {
            transform.SendMessage("Damage",1);
        }
    }
}
