using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    public float totalHP = 1;
    private float hp;
    [HideInInspector] public event UnityAction DeathEvent;
    private void Awake() {
        hp = totalHP;
    }

    public void Damage(float damage)
    {
        totalHP -= damage;
        if(totalHP <= 0)
        {
            if(DeathEvent != null)
                DeathEvent.Invoke();
        }
    }
}
