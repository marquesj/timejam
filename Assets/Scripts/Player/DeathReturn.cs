using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(InputRead))]
public class DeathReturn : MonoBehaviour
{
    public InputLog inputLog;
    private Health health;
    private PlayerSpawner playerSpawner;
    private InputRead inputRead;
    private int selected = 0;
    private bool active = false;
    private GameObject nextSelf;
    private Coroutine changeSelectedRoutine;
    
    private void Awake() 
    {
        health = GetComponent<Health>();
        inputRead = GetComponent<InputRead>();
        health.DeathEvent += Die;
        inputRead.ChangeDirHorizontalEvent += InputDir;
        inputRead.ShootEvent += Select;
    }
    private void Die()
    {
        active = true;
        Time.timeScale = 0;
        selected = 0;
        nextSelf = Instantiate(playerSpawner.playerPrefab,transform.position,Quaternion.identity);
        nextSelf.GetComponent<DeathReturn>().SetSpawner(playerSpawner);
        SaveStartChangeSelectedRoutine();
        StartCoroutine(SelectPastSelf());
    }

    private IEnumerator SelectPastSelf()
    {
        while(true)
        {
            yield return null;
           // Debug.Log(Time.realtimeSinceStartup);
        }
    }

    public void SetSpawner(PlayerSpawner playerSpawner)
    {
        this.playerSpawner = playerSpawner;
        
    }

    private void InputDir(float dir)
    {
        if(!active)return;
      //  Debug.Log("dirInput");
        int  aux = selected;
        selected += -1*(int)dir;
        if(selected < 0)
            selected = 0;
        if(selected >= playerSpawner.clones.Count)
            selected = playerSpawner.clones.Count -1;
        
        if(aux != selected)
            SaveStartChangeSelectedRoutine();
    }

    private void SaveStartChangeSelectedRoutine()
    {
        if(changeSelectedRoutine != null)
            StopCoroutine(changeSelectedRoutine);
        changeSelectedRoutine = StartCoroutine(ChangeSelected(selected));
    }

    private IEnumerator ChangeSelected(int index)
    {
        float percent = 0;
        float startTime = Time.realtimeSinceStartup;
        Vector3 startPos = nextSelf.transform.position;
        float duration = 0.2f;
//            Debug.Log(selected);
        while(percent < 1)
        {
            percent = (Time.realtimeSinceStartup - startTime)/duration;
            nextSelf.transform.position = Vector3.Lerp(startPos, playerSpawner.clones[selected].transform.position, percent);
            yield return null;
        }
    }
    private void Select()
    {
        if(!active)return;
        
        inputLog.RevertTo(Time.time - playerSpawner.clones[selected].GetComponent<InputSimulator>().timeOffset);

        playerSpawner.RemoveClones(selected);

       
        Time.timeScale = 1;

        Vector3 pos = nextSelf.transform.position;
        
        Destroy(nextSelf);
        nextSelf = Instantiate(playerSpawner.playerPrefab,pos,Quaternion.identity);
        nextSelf.GetComponent<DeathReturn>().SetSpawner(playerSpawner);
        Destroy(gameObject);
    }
}
