using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerClonePrefab;
    public float timePeriod = .3f;
    private float currentOffset = 0;
    void Start()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        StartCoroutine(SpawnClonesRoutine());
    }

    private IEnumerator SpawnClonesRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(timePeriod);

            GameObject clone = Instantiate(playerClonePrefab, transform.position, Quaternion.identity);
            currentOffset += timePeriod;        
            clone.GetComponent<InputSimulator>().timeOffset = currentOffset;
        }

    }
}
