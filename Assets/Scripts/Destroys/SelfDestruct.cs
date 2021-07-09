using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float timeToLive = 0;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die",timeToLive);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
