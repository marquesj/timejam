using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheat : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject platformPrefab;
    public float heightDistance;
    public float velocityThreshold;
    public bool waiting = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(rb.velocity.y < velocityThreshold && !waiting)
        {
            Instantiate(platformPrefab, transform.position +Vector3.down*heightDistance + Vector3.right*1f,Quaternion.identity);
            waiting = true;
            Invoke("StopWait", 1);
        }
    }
    private void StopWait()
    {
        waiting = false;
    }
}
