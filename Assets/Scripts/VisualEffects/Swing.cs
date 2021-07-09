using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public float duration;
    public float anglediff;
    public float linger;


    private void Start() {
        StartCoroutine(SwingRoutine());
    }

    private IEnumerator SwingRoutine()
    {
        while(true)
        {
            float percent = 0;
            float startTime = Time.time;
            Quaternion startRotation= transform.rotation;
            Quaternion goalRotation = Quaternion.Euler(0,0,anglediff+5);
            while(percent < 1)
            {
                percent = (Time.time-startTime)/duration;
                transform.rotation = Quaternion.Lerp(startRotation,goalRotation,percent*percent);
                yield return null;
            }
            percent = 0;
            startTime = Time.time;
            startRotation= transform.rotation;
            goalRotation = Quaternion.Euler(0,0,anglediff);
            while(percent < 1)
            {
                percent = (Time.time-startTime)/(duration/10);
                transform.rotation = Quaternion.Lerp(startRotation,goalRotation,percent*percent);
                yield return null;
            }

            yield return new WaitForSeconds(linger);

            percent = 0;
            startTime = Time.time;
            startRotation = transform.rotation;
            goalRotation = Quaternion.Euler(0,0,-anglediff-5);
            while(percent < 1)
            {
                percent = (Time.time-startTime)/duration;

                transform.rotation = Quaternion.Lerp(startRotation,goalRotation,percent*percent);
                yield return null;
            }
            percent = 0;
            startTime = Time.time;
            startRotation= transform.rotation;
            goalRotation = Quaternion.Euler(0,0,-anglediff);
            while(percent < 1)
            {
                percent = (Time.time-startTime)/(duration/10);
                transform.rotation = Quaternion.Lerp(startRotation,goalRotation,percent*percent);
                yield return null;
            }
            yield return new WaitForSeconds(linger);
        }
    }
}
