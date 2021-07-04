using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedElement : MonoBehaviour
{
    public TimeEvents timeEvents;
    public float timeOffset;
    public List<MonoBehaviour> deactivateComponentOnDestroy = new List<MonoBehaviour>();
    public List<GameObject> deactivateObjectOnDestroy = new List<GameObject>();
    private Collider2D[] colliders;
    private List<(float, Vector3)> positions = new List<(float,Vector3)>();
    private float precision = 0.1f;
    private float destructedTime = -1;
    private float createTime;
    private bool destroyed = false;
    private Vector3 creationPos;
    private void Awake() {
        timeEvents.GoBackInTimeEvent += GoBack;
        timeEvents.SaveStateEvent += SavePosition;
        timeEvents.PreviewBackInTimeEvent += PreviewPosition;
        createTime = Time.time;
        colliders = GetComponents<Collider2D>();
        creationPos = transform.position;
        
    }
    public void SetTimeOffset(float n)
    {
        timeOffset = n;
    }

    private void GoBack(float time)
    {
        float offset = (Time.time - time);
        if(time < createTime )
        {
            Destroy(gameObject);
            return;
        }
        SetPastState(time);
        AdjustTimeline(offset);
    }

    private void PreviewPosition(float time)
    {
        SetPastState(time);
    }

    private void SetPastState(float time)
    {
        float offset = (Time.time - time);
        if(time < createTime)
        {
            Debug.Log("destroy");
            //destroyed = true;
            SetAllPartsActive(false);
            return;
        }
        if( destructedTime > time || destructedTime == -1)
        {
           Debug.Log("comming back");
            destroyed = false;
            SetAllPartsActive(true);

        }

        if(positions.Count >= 1 && positions[0].Item1 > time)
        {
            float percent = (time - createTime)/(positions[0].Item1 - createTime);
            transform.position = Vector3.Lerp(creationPos, positions[0].Item2, percent);
        }

        for(int i = 0; i < positions.Count; i++)
        {
            if( positions[i].Item1 < time && positions[i+1].Item1 >= time)
            {
              
               
                float percent = (time - positions[i].Item1)/(positions[i+1].Item1 - positions[i].Item1);
                transform.position = Vector3.Lerp(positions[i].Item2, positions[i+1].Item2,percent);

            }
        }
    }

    private void AdjustTimeline(float offset)
    {
        createTime += offset;
        if(destroyed)
            destructedTime += offset;
        

        for(int i = positions.Count-1; i >= 0; i--)
        {
            if(positions[i].Item1 > Time.time - offset)
                positions.RemoveAt(i);
            else
                positions[i] = (positions[i].Item1 + offset, positions[i].Item2);
        } 
    }

    private void SavePosition()
    {
//        Debug.Log("Saving Pos at" + Time.time);
        positions.Add((Time.time, transform.position));
    }

    public void TimeSafeDestroy()
    {
        destroyed = true;
        destructedTime = Time.time;
        SetAllPartsActive(false);
    }

    private void SetAllPartsActive(bool state)
    {
        foreach(MonoBehaviour behaviour in deactivateComponentOnDestroy)
        {
            behaviour.enabled = state;
        }
        foreach(GameObject obj in deactivateObjectOnDestroy)
        {
            obj.SetActive(state);
        }
        foreach(Collider2D c in colliders)
        {
            c.enabled = state;
        }
    }

    private void OnDestroy() {
        timeEvents.GoBackInTimeEvent -= GoBack;
        timeEvents.SaveStateEvent -= SavePosition; 
        timeEvents.PreviewBackInTimeEvent -= PreviewPosition;
    }
}
