using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckWall : MonoBehaviour
{
    public float distanceToWall = 1;
    public float heightOffset = 1;

    public LayerMask wallLayers;
    public LayerMask jumpableLayers;
    public bool walled = false;
    public bool isJumpable = false;
    public bool isLeft = false;
    public float cooldownTime = 1;
    public float sideOffset = 0.1f;
    
    private CharacterControl characterControl;
    private CheckGround checkGround;

    private Rigidbody2D rb;
    private bool asleep = false;

    public bool pressingLeft = false;
    public bool pressingRight = false;
    private Coroutine leftCooldownRoutine = null;
    private Coroutine rightCooldownRoutine = null;
    private bool inRoutineLeft = false;
    private bool inRoutineRight = false;

    [HideInInspector]public event UnityAction<bool> walledEvent;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        checkGround = GetComponent<CheckGround>();
        characterControl = GetComponent<CharacterControl>();
            
    }
    void FixedUpdate()
    {
        
        if(!asleep)
            UpdateState();

    }
    private void Update() {

            UpdateKeys();
    }


    private void UpdateKeys()
    {
        bool leftPressedRightNow = characterControl.bufferedMovementInput < 0;
        bool rightPressedRightNow = characterControl.bufferedMovementInput > 0;
        if(leftPressedRightNow)
        {   
            if(leftCooldownRoutine != null)
                StopCoroutine(leftCooldownRoutine);
            pressingLeft = true;
        }else if(pressingLeft && ! inRoutineLeft)
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
        bool checkWallLeft  = leftCheck.Item1;// && (pressingLeft) ;
        bool checkWallRight = rightCheck.Item1;// && (pressingRight);
        bool checkWall = (checkWallLeft||checkWallRight) ;
        if(checkGround != null)
            checkWall = checkWall && !checkGround.grounded && GoingDown();
        
        bool previousStateOfisJumpable = isJumpable;
        isJumpable = leftCheck.Item2 || rightCheck.Item2;
        if(!walled && checkWall && GoingDown())
        {
            if(walledEvent != null)
            {
                walledEvent.Invoke(checkWallLeft);
            }
            isLeft = checkWallLeft;
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
        RaycastHit2D hit;
        bool jumpable = false;
        for(int i = 0; i < 3; i++)
        {
            Vector3 pos = transform.position + Vector3.up* heightOffset + Vector3.down*sideOffset *(i-1) ;
            hit = Physics2D.Raycast(pos, dir, distanceToWall, wallLayers);
            Debug.DrawRay(pos, dir * distanceToWall, Color.blue);
            if (hit.collider != null)
            {   
                if(jumpableLayers == (jumpableLayers | (1 << hit.collider.gameObject.layer) ))
                    jumpable = true;
        
                return (true,jumpable);
            }
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
