using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAwayFromPlayer : MonoBehaviour
{
    private GameObject target;
    public float distance = 1;
    public float maxDistance = 50;
    public float startingX;
    private void Start() {
        startingX = transform.position.x;
    }
    private void LateUpdate() {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        Vector3 offset = transform.position - target.transform.position;
        if(offset.magnitude < distance)
            transform.position = new Vector3(target.transform.position.x + distance, transform.position.y,transform.position.z);

        if(transform.position.x - startingX> maxDistance)
        {
            transform.position = new Vector3(startingX + maxDistance,transform.position.y,transform.position.z);

            GetComponent<Door>().SetState(true);
            Destroy(this);

        }


    }
}
