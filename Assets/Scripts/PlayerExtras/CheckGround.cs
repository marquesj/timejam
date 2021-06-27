using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CheckGround : MonoBehaviour
{
    public float distanceToFloor = 1;
    public LayerMask groundLayers;
    public bool grounded = false;
    public float sideOffset = 0.1f;
    public float gracePeriod = 0;
    public float verticalOffset  =0;
    private Coroutine ungroundQueue = null;
 //   private bool queuedUnground = false;

    [HideInInspector]public event UnityAction landedEvent;
    void FixedUpdate()
    {
        bool checkResult = TripleCheck();
        if(!grounded && checkResult /*&& !queuedUnground*/ )
        {
           // MakeSureCoroutineStops(ungroundQueue);
            if(landedEvent != null)
                landedEvent.Invoke();
            grounded = true;
            Debug.Log("landedEvent");
        }
        else if(grounded && !checkResult /*&& ungroundQueue == null*/)
        {
           grounded = false;
           // ungroundQueue = StartCoroutine(QueueUnground(gracePeriod));
            
        }
    }
    private IEnumerator QueueUnground(float time)
    {
      //  queuedUnground = true;
        Debug.Log("starting");
        yield return new WaitForSeconds(time);
        grounded = false;
        Debug.Log("done");
        ungroundQueue = null;
        //queuedUnground = false;
    }
    private void MakeSureCoroutineStops(Coroutine coroutine)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    private void JumpEvent()
    {
      /* if(grounded)return;
        MakeSureCoroutineStops(ungroundQueue);
        grounded = false;
        ungroundQueue = null;
        queuedUnground = false;*/
    }
    private bool TripleCheck()
    {
        Check(-sideOffset);
        Check(sideOffset);
        if(Check(0))
            return true;
        else if(Check(-sideOffset))
            return true;
        else if(Check(sideOffset))
            return true;
        return 
            false;
    }

    private bool Check(float xOffset)
    {
        Vector3 pos = transform.position + Vector3.right*xOffset + Vector3.up * verticalOffset;
        RaycastHit2D hit = Physics2D.Raycast(pos, -Vector2.up, distanceToFloor, groundLayers);
        Debug.DrawRay(pos, Vector3.down * distanceToFloor, Color.blue);
        // If it hits something...
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }
}
