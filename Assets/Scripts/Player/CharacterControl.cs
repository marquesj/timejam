using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CheckGround))]
[RequireComponent(typeof(ShootController))]
[RequireComponent(typeof(CheckWall))]

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
    public float wallSlideSpeed = .8f;
    [Header("Slide Parameters")]
    public float slideForce = 1;

    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public CheckGround checkGround;
    [HideInInspector]public CheckWall checkWall;
    private SpriteRenderer spriteRenderer;
    private ShootController shootController;

    [HideInInspector]public event UnityAction StartRunningEvent;
    [HideInInspector]public event UnityAction StopRunningEvent;
    [HideInInspector]public event UnityAction JumpEvent;
    [HideInInspector]public event UnityAction SlideEvent;
    [HideInInspector]public event UnityAction StopSlideEvent;
    
    [HideInInspector]public bool movementBlock = false;
    [HideInInspector]public float bufferedMovementInput;
    [HideInInspector]public float bufferedVerticalInput;

   [HideInInspector]public bool sliding = false;
    [HideInInspector]public bool bouncing = false;
    private bool running = false;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        checkGround = GetComponent<CheckGround>();
        checkWall = GetComponent<CheckWall>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        shootController = GetComponentInChildren<ShootController>();

        inputGenerator.JumpEvent += Jump;
        inputGenerator.ChangeDirHorizontalEvent += BufferMovement;
        inputGenerator.ChangeDirVerticalEvent += CheckSlide;

        checkGround.bounceEvent += Bounce;
        checkGround.landedEvent += StopBounce;
        checkGround.landedEvent += CheckSlide;
        checkWall.walledEvent += StopBounce;
    }    

    private void FixedUpdate() {
       
        if(!movementBlock)
        {
            if(!(checkGround.grounded && sliding))
            {

                rb.velocity = new Vector2(bufferedMovementInput * speed, rb.velocity.y);
            }
        }
            
        if(checkWall.walled)
        {
            WallSlide();
        }
        else if(!checkGround.grounded)
        {
  
            FastFall();
        }
        
    }

    private void BufferMovement(float dir)
    {

        bufferedMovementInput = dir;
        if(Time.timeScale != 0)
        {
            if(!sliding)
            {
                if(dir == 1)
                    SetRotation(0);
                if(dir == -1)
                    SetRotation(180);

            }

         /*   if(transform.localScale.y == 0.5f)
                transform.localScale = new Vector3(1,1,1);*/

            if(dir != 0 && !running)
            {
                running = true;
                if(StartRunningEvent!=null)
                    StartRunningEvent.Invoke();
            }
            else if(dir == 0 && running)
            {
                running = false;
                if(StopRunningEvent!=null)
                    StopRunningEvent.Invoke();
            }
        }

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
                rb.velocity = Vector2.zero;
                
                if(JumpEvent!=null)
                    JumpEvent.Invoke();
           }else//regular jump
           {
                if(JumpEvent!=null)
                    JumpEvent.Invoke();
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
        }else if(rb.velocity.y > 0 && !inputGenerator.jumpHeld &&  !bouncing)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJump - 1) * Time.deltaTime ;
        }
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * graviyMod);
    }

    private void WallSlide()
    {
        if(checkWall.isLeft)
            SetRotation(0);
        else
            SetRotation(180);
        if(!checkWall.GoingDown())
        {
            if(checkWall.isLeft && bufferedMovementInput < 0)
                rb.velocity = new Vector2(0, rb.velocity.y);
            if(!checkWall.isLeft && bufferedMovementInput > 0)
            {

                 rb.velocity = new Vector2(0, rb.velocity.y);
            }
            return;
        }

        /*if(rb.velocity.x > 0 && !checkWall.isLeft)
            rb.velocity = Vector2.zero;
        if(rb.velocity.x < 0 && checkWall.isLeft)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = new Vector2(rb.velocity.x, 0);*/

       // rb.velocity += Vector2.up * Physics2D.gravity.y * (slide - 1) * Time.deltaTime ;
        rb.velocity = Vector2.right*rb.velocity.x  + Vector2.up * Physics2D.gravity.y * wallSlideSpeed;


    }

    public void Bounce(float bounciness)
    {
        if(inputGenerator.timeOffset != 0)
            return;

        ApplyBounce(bounciness);
        
        inputGenerator.SaveBounceInput(bounciness);
    }
    public void ApplyBounce(float bounciness)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce * bounciness ,ForceMode2D.Impulse);
        bouncing = true;
        shootController.BlockShoot();
    }



    private void BlockMovement()
    {
        movementBlock = true;
    }
    private void UnblockMovement()
    {
        movementBlock = false;
        if(bufferedMovementInput > 0)
            SetRotation(0);
        if(bufferedMovementInput < 0)
            SetRotation(180);
    }

    private void SetRotation(float rot)
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,rot,transform.eulerAngles.z);
    }

    private void CheckSlide(float dir)
    {
        if(Time.timeScale == 0)
            return;
        
        bufferedVerticalInput = dir;

        if(dir == -1 && checkGround.grounded && bufferedMovementInput !=0)
        {
            if(sliding == false)
            {
                rb.AddForce(transform.right * slideForce ,ForceMode2D.Impulse);
             //   transform.localScale = new Vector3(1,.5f,1);
                sliding = true;
                if(SlideEvent!=null)
                    SlideEvent.Invoke();
            }
        }
        else if(dir == -1 && checkGround.grounded && bufferedMovementInput ==0)
        {
      //      transform.localScale = new Vector3(1,.5f,1);
        }
        else
        {
            if(sliding && StopSlideEvent != null)
                StopSlideEvent.Invoke();
            //transform.localScale = new Vector3(1,1,1);
            sliding = false;
            if(bufferedMovementInput == 1)
                SetRotation(0);
            if(bufferedMovementInput == -1)
                SetRotation(180);
        }
    }
    private void CheckSlide()
    {
        sliding = false;
        CheckSlide(bufferedVerticalInput);
    }

    private void StopBounce()
    {
//        Debug.Log(" not bouncuing" );
        bouncing = false;
    }
    private void StopBounce(bool aux)
    {
        StopBounce();
    }
}
