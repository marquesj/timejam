using System.Transactions;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CheckGround : MonoBehaviour
{
    public float distanceToFloor = 1;
    public LayerMask groundLayers;
    public LayerMask bouncyLayers;
    public bool grounded = false;
    public float sideOffset = 0.1f;
    public float gracePeriod = 0;
    public float verticalOffset  =0;
    public float circleRadius = 0.05f;
    public float coyoteTime = 0.1f;
    [HideInInspector]public bool inCoyoteTime = false;
    private Coroutine ungroundQueue = null;
    private Coroutine coyoteTimeRoutine = null;
    private bool blockedCoyoteTime = false;
 //   private bool queuedUnground = false;
    private InputGenerator inputGenerator;

    [HideInInspector]public event UnityAction landedEvent;
    [HideInInspector]public event UnityAction<float> bounceEvent;

    private void Awake() {
        inputGenerator = GetComponent<InputGenerator>();
    }
    private void Start() {
        inputGenerator.JumpEvent += JumpEvent;
    }
    void FixedUpdate()
    {
        bool checkResult = TripleCheck();
        if(!grounded && checkResult /*&& !queuedUnground*/ )
        {
           // MakeSureCoroutineStops(ungroundQueue);
            grounded = true;
            blockedCoyoteTime = false;
            if(landedEvent != null)
                landedEvent.Invoke();
//            Debug.Log("landedEvent");

        }
        else if(grounded && !checkResult /*&& ungroundQueue == null*/)
        {
            grounded = false;
            if(!blockedCoyoteTime)
                ApplyCoyoteTime();
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
      blockedCoyoteTime = true;
      inCoyoteTime = false;
    }
    private bool TripleCheck()
    {
        /*Check(-sideOffset);
        Check(sideOffset);*/
        //CheckBounce();
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
            //Check if Moving platform
            if(hit.collider.tag == "MovingPlatform")
                transform.parent = hit.collider.transform;
            return true;
        }
        transform.parent = null;
        return false;
    }
    private void CheckBounce()
    {
        Vector3 pos = transform.position + Vector3.up * 0.04355644f;
        Collider2D hit = Physics2D.OverlapCapsule(new Vector2(pos.x,pos.y),new Vector2(0.3025943f,0.2520498f), CapsuleDirection2D.Horizontal,0,bouncyLayers);

        if (hit != null )
        {
            BouncyElement bounce = hit.gameObject.GetComponent<BouncyElement>();
            if(bounce == null)
            {
                Debug.Log(hit.gameObject.name + " is missing bouncyElement component");
                return;
            }
            TimedElement timedElement = bounce.GetComponent<TimedElement>();
            if(bounce.bounciness!=0 && IsFromPastOrPresent(timedElement))
            {
                if(bounce.destroyOnBounce)
                    hit.gameObject.GetComponent<Health>().Damage(1);
                if(bounceEvent != null)
                    bounceEvent.Invoke(bounce.bounciness);

            }

        }
    }

    private bool IsFromPastOrPresent(TimedElement timedElement)
    {
        if(timedElement == null)
            return true;
        if(inputGenerator.timeOffset < timedElement.timeOffset) 
            return true;
        return false;
    }

    private void ApplyCoyoteTime()
    {
        blockedCoyoteTime = true;
        inCoyoteTime = true;
        if(coyoteTimeRoutine != null)
            StopCoroutine(coyoteTimeRoutine);
        coyoteTimeRoutine = StartCoroutine(StopCoyoteTimeRoutine());

    }
    private IEnumerator StopCoyoteTimeRoutine()
    {
        yield return new WaitForSeconds(coyoteTime);
        inCoyoteTime = false;

    }


    private void OnTriggerEnter2D(Collider2D other) {
        if( bouncyLayers == (bouncyLayers | (1 << other.gameObject.layer)))
        {
            CheckBounce();
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
        if( bouncyLayers == (bouncyLayers | (1 << other.gameObject.layer)))
        {
            CheckBounce();
        }
    }

}
