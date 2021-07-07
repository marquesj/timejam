using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TimedElement))]
public class Key : MonoBehaviour
{
    public Door door;
    private TimedElement timedElement;
    void Start()
    {
        if(door == null)
        {
            Destroy(this);
            return;
        }
        timedElement = GetComponent<TimedElement>();
        timedElement.SetStateEvent += SetDoor;
        
    }

    private void SetDoor(bool state)
    {
        door.SetState(!state);
    }
}
