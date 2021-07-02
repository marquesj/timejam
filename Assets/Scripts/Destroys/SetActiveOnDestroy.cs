using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnDestroy : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    
    private void OnDestroy() {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(true);
        }
    }
}
