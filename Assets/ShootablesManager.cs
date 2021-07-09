using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootablesManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Door door;
    void Start()
    {
        if(door == null)
        {
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(transform.childCount == 0) {
            door.SetState(true);
        }

    }

    private void SetDoor(bool state)
    {
        door.SetState(!state);
    }
}

