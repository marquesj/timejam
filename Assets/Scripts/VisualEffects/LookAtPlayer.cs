using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject eye;
    public float eyeRadius = 0.1f;
    private Vector3 unitVector;
    private Vector3 eyeCenter;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        eyeCenter = eye.transform.position;
    }

    void Update()
    {
        if(target == null)
            target = GameObject.FindGameObjectWithTag("Player");
        if(target!=null)
        {
            Look();
        }

    }
    private void Look()
    {
        Vector3 offset = target.transform.position - eyeCenter;
         unitVector = offset.normalized;
        
        eye.transform.position  = eyeCenter+unitVector*eyeRadius;


    }


}
