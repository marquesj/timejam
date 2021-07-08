using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(InputRead))]
public class DeathReturn : MonoBehaviour
{
    public GameObject cursorPrefab;
    public TimeEvents timeEvents;
    public InputLog inputLog;
    private Health health;
    private PlayerSpawner playerSpawner;
    private InputRead inputRead;
    private int selected = 0;
    private bool active = false;
    private GameObject nextSelf;
    private Coroutine changeSelectedRoutine;
    private bool midShift = false;

    public AudioSource navigateSound;

    public AudioSource dieSound;

    
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
        dieSound.Play();
        timeEvents.RaiseStopTimeEvent();
        if(playerSpawner.clones.Count == 0)
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);

        }
        active = true;
        Time.timeScale = 0;
        selected = 0;
        nextSelf = Instantiate(cursorPrefab,transform.position,transform.rotation);

        playerSpawner.SetPlayer(nextSelf);
       /* nextSelf.GetComponent<DeathReturn>().SetSpawner(playerSpawner);
        nextSelf.GetComponentInChildren<Animator>().SetTrigger("Die");*/
        SaveStartChangeSelectedRoutine();
    }



    public void SetSpawner(PlayerSpawner playerSpawner)
    {
        this.playerSpawner = playerSpawner;
        playerSpawner.SetPlayer(gameObject);
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
        float newTime = Time.time - playerSpawner.cloneInputs[selected].timeOffset;
        timeEvents.RaisePreviewBackInTimeEvent(newTime);
    }

    private IEnumerator ChangeSelected(int index)
    {
        navigateSound.Play();
        midShift = true;
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
        midShift = false;
    }
    private void Select()
    {
        if(!active || midShift)return;
        float newTime = Time.time - playerSpawner.cloneInputs[selected].timeOffset;
        inputLog.RevertTo(newTime);
        timeEvents.RaiseGoBackInTimeEvent(newTime);

        playerSpawner.RemoveClones(selected);

       
        Time.timeScale = 1;

        Vector3 pos = nextSelf.transform.position;
        
        Destroy(nextSelf);
        nextSelf = Instantiate(playerSpawner.playerPrefab,pos,Quaternion.identity);
        nextSelf.GetComponent<DeathReturn>().SetSpawner(playerSpawner);
        Destroy(gameObject);

        timeEvents.RaiseContinueTimeEvent();
    }

    public GameObject getNextSelf(){
        return nextSelf;
    }
}
