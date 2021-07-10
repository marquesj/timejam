using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public TimeEvents timedEvents;
    public GameObject eye;
    public float eyeRadius = 0.1f;
    public GameObject projectilePrefab;
    public float minShootInterval;
    public float maxShootInterval;
    private Vector3 eyeCenter;
    private GameObject target;
    private Vector3 unitVector;
    private Health health;
    public Slider slider;

    public List<(float,float)> hpOverTime = new List<(float,float)>();
    public GameObject bossdeadPrefab;
    void Start()
    {
        health = GetComponent<Health>();
        health.DeathEvent += Die;
        health.DamageEvent += UpdateHealthBar;
        eyeCenter = eye.transform.position;
        StartCoroutine(Shoot());

        timedEvents.GoBackInTimeEvent+=GoBack;
        timedEvents.PreviewBackInTimeEvent+=Preview;
    }
    private void OnDestroy() {
        timedEvents.GoBackInTimeEvent-=GoBack;
        timedEvents.PreviewBackInTimeEvent-=Preview;
    }
    // Update is called once per frame
    void Update()
    {
        if(target == null)
            target = GameObject.FindGameObjectWithTag("Player");
        if(target!=null)
        {
            LookAtPlayer();
        }

    }

    private void LookAtPlayer()
    {
        Vector3 offset = target.transform.position - eyeCenter;
         unitVector = offset.normalized;
        
        eye.transform.position  = eyeCenter+unitVector*eyeRadius;


    }

    private IEnumerator Shoot()
    {
        while(true)
        {
            float waitTime = Random.Range(minShootInterval,maxShootInterval);
            yield return new WaitForSeconds(waitTime);


            float angle = 0;
            angle = Vector3.Angle(Vector3.right, unitVector);
            if(unitVector.y <0)
                angle *=-1;
            

//            Debug.Log(angle);
            Instantiate(projectilePrefab,eye.transform.position,Quaternion.Euler(0,0,angle));
        }
    }
    private void Die()
    {
        foreach(GameObject clone in GameObject.FindGameObjectsWithTag("EnemyProjectile"))
            Destroy(clone);

        Destroy(GameObject.Find("PlayerSpawn"));
        foreach(GameObject clone in GameObject.FindGameObjectsWithTag("Clone"))
            Destroy(clone);

        Instantiate(bossdeadPrefab,transform.position,transform.rotation);
    	Destroy(gameObject);



    }
    private void UpdateHealthBar()
    {
        slider.value = health.hp/health.totalHP;

        hpOverTime.Add((Time.time, health.hp));
    }

    private void GoBack(float time)
    {
        float offset = (Time.time - time);
        SetPastState(time);
        AdjustTimeline(offset);
        slider.value = health.hp/health.totalHP;
    }
    private void Preview(float time)
    {
        SetPastState(time);
        slider.value = health.hp/health.totalHP;
    }

    private void SetPastState(float time)
    {
        float offset = (Time.time - time);

        for(int i = 0; i < hpOverTime.Count-1; i++)
        {
            if( hpOverTime[i].Item1 < time && (i == hpOverTime.Count-1 || hpOverTime[i+1].Item1 >= time))
            {
              
               
                health.hp = hpOverTime[i].Item2;

            }
        }
    }

    private void AdjustTimeline(float offset)
    {

        

        for(int i = hpOverTime.Count-1; i >= 0; i--)
        {
            if(hpOverTime[i].Item1 > Time.time - offset)
                hpOverTime.RemoveAt(i);
            else
                hpOverTime[i] = (hpOverTime[i].Item1 + offset, hpOverTime[i].Item2);
        } 
    }
}
