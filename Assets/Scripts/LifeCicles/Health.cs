using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    public float totalHP = 1;
    public float hp;
    [HideInInspector] public event UnityAction DeathEvent;
    [HideInInspector] public event UnityAction DamageEvent;
    private void Awake() {
        hp = totalHP;
    }
    private void OnEnable() {
        hp = totalHP;
    }

    public void Damage(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            if(DeathEvent != null)
                DeathEvent.Invoke();
        }else
        {
            if(DamageEvent != null)
                DamageEvent.Invoke();

        }
    }
}
