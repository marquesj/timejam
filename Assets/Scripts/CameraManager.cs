using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    public PlayerSpawner spawner = null;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
        GameObject aux = GameObject.Find("PlayerSpawn");
        if(spawner == null && aux!=null)
            spawner = aux.GetComponent<PlayerSpawner>();
        if(spawner == null)
            target = GameObject.FindGameObjectWithTag("Player");
    }   

    // Update is called once per frame
    void Update()
    {
        // TODO: subscribe to changes instead of checking every frame
        if(spawner != null){
            target = spawner.GetPlayer();
            if(target == null)
                target = spawner.gameObject;
            //player.GetComponent<DeathReturn>().getNextSelf();
        }
      
            cam.Follow = target.transform;
    }
}
