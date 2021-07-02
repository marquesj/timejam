using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CheckGround))]
//[RequireComponent(typeof(CheckWall))]

public class CharacterControl : MonoBehaviour
{
    public InputGenerator inputGenerator;
    [Header("Movement Parameters")]
    public float speed = 1;
    [Header("Jump Parameters")]
    public float jumpForce = 1;
    public float graviyMod = 1;
    public float fallMultiplier = 1;
    public float lowJumpMultiplier = 1;
    [Header("Wall Jump Parameters")]
    public float wallJumpHorizontalForce = 1;
    public float wallJumpForce = 1;
    public float wallJumpBlockMovementTime = 0.2f;

    private Rigidbody2D rb;
    private CheckGround checkGround;
    private CheckWall checkWall;
    private SpriteRenderer spriteRenderer;
    [HideInInspector]public bool movementBlock = false;
    [HideInInspector]public float bufferedMovementInput;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        checkGround = GetComponent<CheckGround>();
        checkWall = GetComponent<CheckWall>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        inputGenerator.JumpEvent += Jump;
        inputGenerator.ChangeDirHorizontalEvent += BufferMovement;

        checkGround.bounceEvent += Bounce;
    }    

    private void FixedUpdate() {
       
        if(!movementBlock)
        {
            rb.velocity = new Vector2(bufferedMovementInput * speed, rb.velocity.y);

        }
            
        if(checkWall.walled)
        {
            if(rb.velocity.x > 0 && !checkWall.isLeft)
                rb.velocity = Vector2.zero;
            if(rb.velocity.x < 0 && checkWall.isLeft)
                rb.velocity = Vector2.zero;
            else
                rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * Physics2D.gravity.y * (0.5f - 1) * Time.deltaTime ;
        }
        if(!checkGround.grounded)
        {
  
            FastFall();
        }
        
    }

    private void BufferMovement(float dir)
    {

        bufferedMovementInput = dir;

    }

    private void Jump()
    {
        if((checkGround.grounded || checkWall.walled))
        {
            Vector2 dir = jumpForce * Vector2.up;
            
           if(checkWall.walled)//wall jump
           {
                dir = wallJumpForce * Vector2.up;
                checkWall.Sleep(0.1f);
                float dirMod = 1;
                if(!checkWall.isLeft)
                    dirMod = -1;
                dir += wallJumpHorizontalForce * Vector2.right * dirMod;

                BlockMovement();
                Invoke("UnblockMovement",wallJumpBlockMovementTime);

           }
//            Debug.Log(dir);
            rb.AddForce(dir ,ForceMode2D.Impulse);
           // bufferedImpulse = bufferedMovementInput*speed*horizontalSpeedModifier;
           // hasTurnedInAir = false;

        }
    }
    private void FastFall()
    {
        float fallMult = fallMultiplier;
        float lowJump = lowJumpMultiplier;

        if(rb.velocity.y < 0 )
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime ;
        }else if(rb.velocity.y > 0 && !inputGenerator.jumpHeld)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJump - 1) * Time.deltaTime ;
        }
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * graviyMod);
    }

    private void Bounce(float bounciness)
    {
        rb.AddForce(Vector2.up * jumpForce * bounciness ,ForceMode2D.Impulse);
    }

    private void BlockMovement()
    {
        movementBlock = true;
    }
    private void UnblockMovement()
    {
        movementBlock = false;
    }

}
