using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask layermask;

    private bool active = false;
    private void OnTriggerEnter2D(Collider2D other) {
        UnityEngine.Debug.Log("col");
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            active = true;
            transform.localScale = new UnityEngine.Vector3(1f,1f,1f);
            Invoke("Deactivate", 2f);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        UnityEngine.Debug.Log("col");
        if( layermask == (layermask | (1 << other.gameObject.layer)))
        {
            active = true;
            transform.localScale = new UnityEngine.Vector3(1f,1f,1f);
        }        
    }

    private IEnumerator Deactivate(float time) 
    {
        yield return new WaitForSeconds(time);

        active = false;
        transform.localScale = new UnityEngine.Vector3(8f,1f,1f);
    }
}
