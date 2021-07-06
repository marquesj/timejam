using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneTrailHandler : MonoBehaviour
{
    public float cloneFrequency = 0.1f;
    public GameObject clonePrefab;
    public TimeEvents timeEvents;
    private Coroutine trailRoutine;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        timeEvents.SlowTimeEvent += StartTrail;
        timeEvents.RestoreTimeEvent += StopTrail;
        spriteRenderer = GetComponent<SpriteRenderer>();


    }
    private void OnDestroy() {
        timeEvents.SlowTimeEvent -= StartTrail;
        timeEvents.RestoreTimeEvent -= StopTrail;
    }

    private void StartTrail()
    {
        trailRoutine = StartCoroutine(GenerateTrail());
    }
    private void StopTrail()
    {
        Debug.Log("stopping");
        if(trailRoutine != null)
            StopCoroutine(trailRoutine);
    }

    private IEnumerator GenerateTrail()
    {
        while(true)
        {
            GameObject clone = Instantiate(clonePrefab,transform.position,transform.rotation);
            SpriteRenderer cloneSprite = clone.GetComponent<SpriteRenderer>();
            cloneSprite.sprite = spriteRenderer.sprite;
            yield return new WaitForSecondsRealtime(cloneFrequency);
        }

    }
}
