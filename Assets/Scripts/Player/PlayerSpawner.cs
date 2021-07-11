using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public TimeEvents timeEvents;
    public GameObject playerPrefab;
    public GameObject playerClonePrefab;
    public float timePeriod = .3f;
    private int currentOffset = 0;

    public ParticleSystem explosionParticles;
    public List<GameObject> clones = new List<GameObject>();
    [HideInInspector]public List<InputSimulator> cloneInputs = new List<InputSimulator>();
    private GameObject player;
    private AudioSource audioSource;
    private LineRenderer lineRenderer;
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(SpawnClonesRoutine());




        for(int i = 0 ; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, transform.position);
        }
        lineRenderer.enabled = false;
    }
    private void OnEnable() {
        timeEvents.StopTimeEvent += DrawLine;
        timeEvents.ContinueTimeEvent += RemoveLine;
    }
    private void OnDisable() {
        timeEvents.StopTimeEvent -= DrawLine;
        timeEvents.ContinueTimeEvent -= RemoveLine;
    }
    private IEnumerator SpawnClonesRoutine()
    {
        yield return new WaitForSeconds(0.05f);
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem p in particleSystems)
            p.Play();
        player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        player.GetComponent<DeathReturn>().SetSpawner(this);
        timeEvents.RaiseSaveStateEvent();
        while(true)
        {
            yield return new WaitForSeconds(timePeriod);

            GameObject clone = Instantiate(playerClonePrefab, transform.position, Quaternion.identity);
            currentOffset += 1;     

            InputSimulator inputSimulator = clone.GetComponent<InputSimulator>();
            cloneInputs.Add(inputSimulator);   
            inputSimulator.timeOffset = currentOffset*timePeriod;
            clones.Add(clone);
//            Debug.Log("createdAt "+Time.time);
            timeEvents.RaiseSaveStateEvent();

            audioSource.Play();
            if(explosionParticles!=null)
                explosionParticles.Play();
        }

    }
    public void RemoveClones(int index)
    {
        for(int i = 0; i <= index; i++)
        {
            Destroy(clones[i]);
        }
      
        for(int i = 0; i <= index; i++)
        {
            clones.RemoveAt(0);
            cloneInputs.RemoveAt(0);
        }
        for(int i = 0; i < cloneInputs.Count; i++)
        {
            cloneInputs[i].timeOffset -= timePeriod * (index+1);
        }
        currentOffset -= (index+1);
    }

    internal void SetPlayer(GameObject nextSelf)
    {
        player = nextSelf;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    private void DrawLine()
    {
        int i = 0;
        lineRenderer.enabled = true;
        foreach(GameObject clone in clones)
        {
            lineRenderer.SetPosition(i, clone.transform.position + Vector3.up * 0.2f);
            i++;
        }
    }
    private void RemoveLine()
    {
  
        for(int i = 0 ; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, transform.position);
        }
        lineRenderer.enabled = false;
    }
}
