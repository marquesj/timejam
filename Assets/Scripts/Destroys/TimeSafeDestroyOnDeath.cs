using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TimedElement))]
public class TimeSafeDestroyOnDeath : MonoBehaviour
{
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
    }
}
