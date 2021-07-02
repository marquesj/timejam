using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerClonePrefab;
    public float timePeriod = .3f;
    private int currentOffset = 0;
    public List<GameObject> clones = new List<GameObject>();
    [HideInInspector]private List<InputSimulator> cloneInputs = new List<InputSimulator>();
    private GameObject player;
    void Start()
    {
        player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        player.GetComponent<DeathReturn>().SetSpawner(this);

        StartCoroutine(SpawnClonesRoutine());
    }

    private IEnumerator SpawnClonesRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(timePeriod);

            GameObject clone = Instantiate(playerClonePrefab, transform.position, Quaternion.identity);
            currentOffset += 1;     

            InputSimulator inputSimulator = clone.GetComponent<InputSimulator>();
            cloneInputs.Add(inputSimulator);   
            inputSimulator.timeOffset = currentOffset*timePeriod;
            clones.Add(clone);
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
}
