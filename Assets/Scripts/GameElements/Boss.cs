using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject eye;
    public float eyeRadius = 0.1f;
    private Vector3 eyeCenter;
    private GameObject target;
    void Start()
    {
        eyeCenter = eye.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            target = GameObject.FindGameObjectWithTag("Player");
        if(target!=null)
        {
            LookAtPlayer();
        }

    }

    private void LookAtPlayer()
    {
        Vector3 offset = target.transform.position - eyeCenter;
        Vector3 unitVector = offset.normalized;
        
        transform.position  = eyeCenter+unitVector*eyeRadius;
        

    }
}
