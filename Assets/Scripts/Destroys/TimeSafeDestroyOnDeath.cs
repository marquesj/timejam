using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TimedElement))]
public class TimeSafeDestroyOnDeath : MonoBehaviour
{
    public List<GameObject> spawnOnDestroyPrefabs = new List<GameObject>();
    private Health health;
    private TimedElement timedElement;
    private void Awake() 
    {
        health = GetComponent<Health>();
        timedElement = GetComponent<TimedElement>();
        health.DeathEvent += Die;
    }
    private void Die()
    {
        timedElement.TimeSafeDestroy();
        foreach(GameObject obj in spawnOnDestroyPrefabs)
            Instantiate(obj,transform.position,transform.rotation);
    }
}
