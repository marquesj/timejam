using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckWall : MonoBehaviour
{
    public float distanceToWall = 1;

    public LayerMask wallLayers;
    public LayerMask jumpableLayers;
    public bool walled = false;
    public bool isJumpable = false;
    public bool isLeft = false;
    public float cooldownTime = 1;
    public bool playerMode = true;
    
    [HideInInspector]public Controls playerControls;
    [HideInInspector]public CheckGround checkGround;

    private Rigidbody2D rb;
    private bool asleep = false;

    private bool pressingLeft = false;
    private bool pressingRight = false;
    private Coroutine leftCooldownRoutine = null;
    private Coroutine rightCooldownRoutine = null;
    private bool inRoutineLeft = false;
    private bool inRoutineRight = false;

    [HideInInspector]public event UnityAction<bool> walledEvent;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if(!playerMode)
        {
            pressingLeft = true;
            pressingRight = true;
        }
            
    }
    void FixedUpdate()
    {
        
        if(!asleep)
            UpdateState();

    }
    private void Update() {
        if(playerMode)
            UpdateKeys();
    }


    private void UpdateKeys()
    {
        bool leftPressedRightNow = playerControls.player.Horizontal.ReadValue<float>() < 0;
        bool rightPressedRightNow = playerControls.player.Horizontal.ReadValue<float>() > 0;
        if(leftPressedRightNow)
        {   
            if(leftCooldownRoutine != null)
                StopCoroutine(leftCooldownRoutine);
            pressingLeft = true;
        }else if(pressingLeft&& ! inRoutineLeft)
        {
            leftCooldownRoutine = StartCoroutine(LeftKeyCooldown(cooldownTime));
         
        }

        if(rightPressedRightNow )
        {   
            if(rightCooldownRoutine != null)
                StopCoroutine(rightCooldownRoutine);
            pressingRight = true;
        }else if(pressingRight && ! inRoutineRight)
        {
            rightCooldownRoutine = StartCoroutine(RightKeyCooldown(cooldownTime));
        }
    }

    private IEnumerator LeftKeyCooldown( float time)
    {
        inRoutineLeft = true;
        yield return new WaitForSeconds(time);
        pressingLeft = false;
        inRoutineLeft = false;
    }
    private IEnumerator RightKeyCooldown( float time)
    {
        inRoutineRight = true;
        yield return new WaitForSeconds(time);
        pressingRight = false;
        inRoutineRight = false;
    }


    private void UpdateState()
    {
        (bool,bool) leftCheck = Check(Vector2.left);
        (bool,bool) rightCheck = Check(Vector2.right);
        bool checkWallLeft  = leftCheck.Item1 && pressingLeft ;
        bool checkWallRight = rightCheck.Item1 && pressingRight;
        bool checkWall = (checkWallLeft||checkWallRight) ;
        if(checkGround != null)
            checkWall = checkWall && !checkGround.grounded;
        
        bool previousStateOfisJumpable = isJumpable;
        isJumpable = leftCheck.Item2 || rightCheck.Item2;
        if(!walled && checkWall && GoingDown())
        {
            if(walledEvent != null)
            {
                walledEvent.Invoke(checkWallLeft);
                isLeft = checkWallLeft;
            }
            walled = true;
        }
        else if(walled && !checkWall)
        {
            walled = false;
        }
        else if(walled && checkWall && isJumpable != previousStateOfisJumpable)
        {
            walledEvent.Invoke(checkWallLeft);
            Debug.Log("sewitch");
        }
  
    }

    private bool GoingDown()
    {
        if(rb == null) return true;
        if(rb.velocity.y <= 0) return true;
        return false;
    }

    private (bool,bool) Check(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distanceToWall, wallLayers);
        Debug.DrawRay(transform.position, dir * distanceToWall, Color.blue);
        bool jumpable = false;
        // If it hits something...
        if (hit.collider != null)
        {
            if(jumpableLayers == (jumpableLayers | (1 << hit.collider.gameObject.layer) ))
                jumpable = true;
        
            return (true,jumpable);
        }

        return (false,jumpable);
    }

    public void Sleep(float time)
    {
        walled = false;
        asleep=true;
        Invoke("WakeUp",time);
    }
    private void WakeUp()
    {
        asleep = false;
    }
}
