using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class MoveForwards : MonoBehaviour
{
    public float speed;
    
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;    
    }


}
