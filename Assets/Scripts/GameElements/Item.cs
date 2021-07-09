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

        Destroy(other.gameObject);
      
        Destroy(GetComponent<Swing>());
        transform.rotation = Quaternion.identity;
        transform.position = transform.position + Vector3.up*0.5f;

        GetComponentInChildren<Light2D>().enabled = true;

        characterSprite.SetActive(true);
        StartCoroutine(Animation());
    }
    private IEnumerator Animation()
    {

        yield return new WaitForSeconds(1);
        /*animatorInterface.enabled = true;
        inputRead.enabled = true;*/
        GetComponent<Collider2D>().enabled = false;
        Instantiate(playerPrefab,transform.position- Vector3.up*-0.62f,Quaternion.identity);
        door.SetState(true);
        Destroy(gameObject);
    }
}
