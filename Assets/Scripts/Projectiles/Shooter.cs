using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public void Shoot()
    {
        Instantiate(projectilePrefab,transform.position,transform.rotation);
    }
}
