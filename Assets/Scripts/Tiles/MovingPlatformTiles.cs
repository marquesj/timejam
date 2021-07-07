using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTiles : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> points;
    public Transform platform;
    int goalPoint=0;
    public float duration = 1.5f;
    private float speed=0.5f;

    void Start() {
        platform.position = points[0].position;
        speed = UnityEngine.Vector2.Distance(points[0].position, points[1].position) / (duration/2);
    }
    void Update()
    {
        MoveTowardsPoint();
    }

    private void MoveTowardsPoint() {

        platform.position = UnityEngine.Vector2.MoveTowards(platform.position, points[goalPoint].position, Time.deltaTime*speed);

        if(UnityEngine.Vector2.Distance(platform.position, points[goalPoint].position)<0.1f)
        {
            //If so change goal point to the next one
            //Check if we reached the last point, reset to first point
            if (goalPoint == points.Count - 1)
                goalPoint = 0;
            else
                goalPoint++;            
        }
    }
}
