using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    public PlayerSpawner spawner = null;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
        if(spawner == null)
            spawner = GameObject.Find("PlayerSpawn").GetComponent<PlayerSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: subscribe to changes instead of checking every frame
        if(spawner != null){
            GameObject player = spawner.GetPlayer();
            //player.GetComponent<DeathReturn>().getNextSelf();
            cam.Follow = player.transform;
        }
    }
}
