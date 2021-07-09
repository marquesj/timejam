using System.Numerics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigNoseEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public TimeEvents timeEvents;
    public float shootingPeriod = 0.5f;
    public Sprite sprite;
    public LayerMask layermask;
    private bool isRightSprite;
    private SpriteRenderer spriteRenderer;
    public float speed = 1.0f;
    private List<(int, float)> records = new List<(int, float)>(); //int: 0 - left ; 1 - right

    private Collider2D fireCollider;
    private Coroutine disableFireRoutine;

    public AudioSource fireSound;
    public AudioSource turnSound;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        isRightSprite = false;
        timeEvents.GoBackInTimeEvent += GoBack;
        timeEvents.PreviewBackInTimeEvent += PreviewPosition;

        fireCollider = transform.GetChild(0).GetComponent<Collider2D>();

    }
    private void OnDestroy() {
        timeEvents.GoBackInTimeEvent -= GoBack;
        timeEvents.PreviewBackInTimeEvent -= PreviewPosition;
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
        if( layermask == (layermask | (1 << other.gameObject.layer))) {
            
            turnSound.Play();
            if(!isRightSprite) {
                isRightSprite = true;
                records.Add((1, Time.time));
                reposition(true);
                
            }
            else {
                records.Add((0, Time.time));
                isRightSprite = false;
                reposition(false);
                
            }
           
        }

    }
    private IEnumerator DisableFire()
    {
        fireSound.Stop();
        fireCollider.enabled = false;
        yield return new WaitForSeconds(0.3f);
        fireCollider.enabled = true;
        fireSound.Play();
    }

    private void reposition(bool right) {
        if(!right)
            transform.rotation = UnityEngine.Quaternion.Euler(0f,0f,0f);
        else
            transform.rotation = UnityEngine.Quaternion.Euler(0f,180f,0f);

        if(disableFireRoutine!=null)
            StopCoroutine(disableFireRoutine);
        
        disableFireRoutine = StartCoroutine(DisableFire());
    }
    private void GoBack(float time) {
        float offset = (Time.time - time);
        SetPastState(time);
        AdjustTimeline(offset);
    }

    private void PreviewPosition(float time) {
        
        SetPastState(time);
    }

    private void SetPastState(float time)
    {
        for(int i = 0; i < records.Count; i++) {
            if(time < records[i].Item2)
            {
                
                if(records[i].Item1 == 0) {
                    isRightSprite = false;
                    reposition(false);
                }
                else {
                    isRightSprite = true;
                    reposition(true);
                }  
                return;
            }
            else
            {
                if(records[i].Item1 == 0) {
                    isRightSprite = false;
                    reposition(false);
                    
                }
                else {
                    isRightSprite = true;
                    reposition(true);
                }  
            }
            
        }
    }

    private void AdjustTimeline(float offset)
    {
        float createdTime = 0;
        for(int i = 0; i < records.Count; i++) {
            createdTime = records[i].Item2;
            createdTime += offset;
            records[i] = (records[i].Item1, createdTime);
        }
    }
}

