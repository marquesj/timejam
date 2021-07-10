using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;
public class Item : MonoBehaviour
{
    public Door door;
    public GameObject playerPrefab;
    public GameObject characterSprite;
    private AnimatorInterface animatorInterface;
    private InputRead inputRead;
    private Animator animator;
    public AudioSource cutsceneSoundtrack;
    public AudioSource pickupSound;
    private Light2D mLight;
    private void OnTriggerEnter2D(Collider2D other) {
       /* animatorInterface = other.gameObject.GetComponentInChildren<AnimatorInterface>();
        animatorInterface.enabled = false;
        inputRead = other.gameObject.GetComponentInChildren<InputRead>();
        inputRead.enabled = false;
        
        animator =other.gameObject.GetComponentInChildren<Animator>();
        animator.SetTrigger("GetItem");
        animator.enabled = false;

        other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        other.gameObject.transform.position = transform.position;*/
        pickupSound.Play();
        Destroy(other.gameObject);
      
        Destroy(GetComponent<Swing>());
        transform.rotation = Quaternion.identity;
        //transform.position = transform.position + Vector3.up*0.5f;

        mLight = GetComponentInChildren<Light2D>();
        mLight.enabled = true;
        mLight.intensity = 0;
        characterSprite.SetActive(true);
        characterSprite.transform.parent = null;
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<MusicManager>().PlaySecondary();
        StartCoroutine(Animation());
    }
    private IEnumerator Animation()
    {
        float startTime = Time.time;
        float percent = 0;
        float duration = 8;
        Vector3 startingPos = transform.position;
        Vector3 goalPos = transform.position + Vector3.up*0.5f;
        while(percent <1)
        {
            percent = (Time.time - startTime)/duration;
            transform.position = Vector3.Lerp(startingPos,goalPos,percent);
            mLight.intensity = Mathf.Lerp(0,2,percent) + 0.4f*Mathf.Cos(Time.time);
            yield return null;
        }


        yield return new WaitForSeconds(1);
        startTime = Time.time;
        percent = 0;
        duration = 1;
        while(percent <1)
        {
            percent = (Time.time - startTime)/duration;
            
            mLight.intensity = Mathf.Lerp(2,0,percent*percent) + 0.4f*Mathf.Cos(Time.time);
            yield return null;
        }


        GetComponent<TextWriter>().Begin();
        yield return new WaitForSeconds(15);

        /*animatorInterface.enabled = true;
        inputRead.enabled = true;*/
        GetComponent<Collider2D>().enabled = false;
        Instantiate(playerPrefab,characterSprite.transform.position,Quaternion.identity);
        door.SetState(true);
        Destroy(characterSprite);
        Destroy(gameObject);
    }
}
