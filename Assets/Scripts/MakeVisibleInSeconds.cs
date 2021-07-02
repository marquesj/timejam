using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisibleInSeconds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Invoke("MakeVisible",0.4f);
    }

    public void MakeVisible() 
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
