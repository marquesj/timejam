using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedElement : MonoBehaviour
{
    public TimeEvents timeEvents;
    public float timeOffset =0;
    public List<MonoBehaviour> deactivateComponentOnDestroy = new List<MonoBehaviour>();
    public List<GameObject> deactivateObjectOnDestroy = new List<GameObject>();
    public List<GameObject> activateObjectOnDestroy = new List<GameObject>();
   // public List<TimedElement> deactivateTimedElementOnDestroy  = new List<TimedElement>();
    public List<TimedElement> activateTimedElementOnDestroy = new List<TimedElement>();
    public bool startDisabled = false;
    public bool canBeDestroyed = true;
    private Collider2D[] colliders;
    private List<(float, Vector3)> positions = new List<(float,Vector3)>();
    private float precision = .5f;
    private float destructedTime = -1;
    private float createTime=-1;
    private bool destroyed = false;
    private Vector3 creationPos;

    
    
    [HideInInspector]public event UnityAction<bool> SetStateEvent;
    private void Awake() {

        colliders = GetComponents<Collider2D>();
        creationPos = transform.position;

        if(startDisabled)
        {
            TimeSafeDestroy();
            destructedTime = -1;
            //gameObject.SetActive(false);
        }
        else
            createTime = Time.time;
//        Debug.Log("HI");
        TransferTimedElementGameObjects(activateTimedElementOnDestroy,activateObjectOnDestroy);
        //TransferTimedElementGameObjects(deactivateTimedElementOnDestroy,deactivateObjectOnDestroy);
    }
    private void OnEnable() {
        timeEvents.GoBackInTimeEvent += GoBack;
        timeEvents.SaveStateEvent += SavePosition;
        timeEvents.PreviewBackInTimeEvent += PreviewPosition;
    }
    private void TransferTimedElementGameObjects(List<TimedElement> timeList,List<GameObject> objectList )
    {
        int i = 0;
        while(i < activateObjectOnDestroy.Count)
        {
            TimedElement timedElement = activateObjectOnDestroy[i].GetComponent<TimedElement>();
            if(timedElement != null)
            {
                activateTimedElementOnDestroy.Add(timedElement);
                activateObjectOnDestroy.RemoveAt(i);
            }else
            {
                i++;
            }
        }
    }
    public void SetTimeOffset(float n)
    {
        timeOffset = n;
    }

    private void GoBack(float time)
    {
        float offset = (Time.time - time);
        if(time < createTime && canBeDestroyed)
        {
            Destroy(gameObject);
            return;
        }
        
        SetPastState(time);
        Debug.Log("[" + gameObject.name+"] destroyed at " + destructedTime + " going back to " + time);
        if(destructedTime<=time && destructedTime != -1)
        {
            if(SetStateEvent!=null)
                SetStateEvent.Invoke(false);
        }
        else if(SetStateEvent!=null)
                SetStateEvent.Invoke(true);
        AdjustTimeline(offset);
    }

    private void PreviewPosition(float time)
    {
        SetPastState(time);
    }

    private void SetPastState(float time)
    {
 //Debug.Log("[" + gameObject.name+"]Current time " + time + " destroy at " + destructedTime + " created at " + createTime,gameObject);
     
        if(destructedTime<createTime)
            destructedTime=-1;
        if((time < createTime && createTime !=-1) ||  (time >destructedTime&& destructedTime!=-1)  )
        {
            
            destroyed = true;
            SetAllPartsActive(false);
            /*if(time >destructedTime && destructedTime!=-1)
                if(SetStateEvent!=null)
                    SetStateEvent.Invoke(false);
            else    
                if(SetStateEvent!=null)
                    SetStateEvent.Invoke(true);*/
            return;
        }
        if( (destructedTime > time || destructedTime != -1) || (createTime != -1 && createTime < time && (destructedTime < time || destructedTime != -1)))
        {
           
         //  Debug.Log("comming back");
            destroyed = false;
            SetAllPartsActive(true);

               /* if(SetStateEvent!=null)
                    SetStateEvent.Invoke(true);*/
        
        }

        if(positions.Count >= 1 && positions[0].Item1 > time)
        {
            float percent = (time - createTime)/(positions[0].Item1 - createTime);
            transform.position = Vector3.Lerp(creationPos, positions[0].Item2, percent);
        }

        for(int i = 0; i < positions.Count-1; i++)
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
        if(createTime != -1)
        {
            createTime += offset;
        }

        
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
        if(destroyed)
            return;
        destroyed = true;
        destructedTime = Time.time;
        SetAllPartsActive(false);
        SetTimedElementsActive(false);
        if(SetStateEvent!=null)
            SetStateEvent.Invoke(false);
    }
    public void TimeSafeDestroy2()
    {
        if(destroyed)
            return;
        destroyed = true;
     
        SetAllPartsActive(false);
    }
    public void TimeSafeInstantiate()
    {
        destroyed = false;
        createTime = Time.time;
        SetAllPartsActive(true);
        SetTimedElementsActive(true);

    }
    public void TimeSafeInstantiate2()
    {
        destroyed = false;
        createTime = Time.time;
        SetAllPartsActive(true);
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
        foreach(GameObject obj in activateObjectOnDestroy)
        {
            obj.SetActive(!state);
        }




    }
    private void SetTimedElementsActive(bool state)
    {
        foreach(TimedElement timedElement in activateTimedElementOnDestroy)
        {
            if(state)
                timedElement.TimeSafeDestroy2();
            else    
                timedElement.TimeSafeInstantiate2();
        }
        
    }
    private void OnDisable() {
        timeEvents.GoBackInTimeEvent -= GoBack;
        timeEvents.SaveStateEvent -= SavePosition; 
        timeEvents.PreviewBackInTimeEvent -= PreviewPosition;
    }
}
