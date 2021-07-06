using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InputRead))]
public class SlowDownTime : MonoBehaviour
{
    public TimeEvents timeEvents;
    private float transitionDuration = .2f;
    public float slowdown = 0.5f;
    private InputRead inputRead;
    private Coroutine changeTimeRoutine;
    // Start is called before the first frame update
    void Start()
    {
        inputRead = GetComponent<InputRead>();
        inputRead.SlowTimeEvent += SlowTime;
        inputRead.RestoreTimeEvent += RestoreTime;
    }

    private void OnDisable() {
        inputRead.SlowTimeEvent -= SlowTime;
        inputRead.RestoreTimeEvent -= RestoreTime;     
    }

    private void SlowTime()
    {
        timeEvents.RaiseSlowTimeEvent();
        if(changeTimeRoutine != null)
        {
            StopCoroutine(changeTimeRoutine);
        }
        changeTimeRoutine = StartCoroutine(ChangeTimeScale(slowdown));
    }
    private void RestoreTime()
    {
        timeEvents.RaiseRestoreTimeEvent();
        if(changeTimeRoutine != null)
        {
            StopCoroutine(changeTimeRoutine);
        }
        changeTimeRoutine = StartCoroutine(ChangeTimeScale(1));
    }

    private IEnumerator ChangeTimeScale(float goalScale)
    {
        float startTime = Time.time;
        float initialScale = Time.timeScale;
        float percent = 0;
        while(percent < 1)
        {
            if(Time.timeScale == 0)
                yield break;
            percent = (Time.time - startTime)/transitionDuration;
//            Debug.Log(transitionDuration);
            Time.timeScale = Mathf.Lerp(initialScale, goalScale, percent );
            yield return null;
        }
    }
}
