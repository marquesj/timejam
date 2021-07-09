
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    // Start is called before the first frame update

    public float a;
    public float b;
    public float speed;
    private Vector3 initialPosition;
    void Start()
    {
       initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPosition + new Vector3(a * Mathf.Cos(Time.time * speed), b * Mathf.Sin(Time.time * speed), 0f);

    }
}
