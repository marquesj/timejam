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
    [HideInInspector]public event UnityAction WallJumpEvent;
    [HideInInspector]public event UnityAction SlideEvent;
    [HideInInspector]public event UnityAction StopSlideEvent;
    [HideInInspector]public event UnityAction BounceEvent;
    
    
    [HideInInspector]public bool movementBlock = false;
    [HideInInspector]public float bufferedMovementInput;
    [HideInInspector]public float bufferedVerticalInput;

   [HideInInspector]public bool sliding = false;
    [HideInInspector]public bool bouncing = false;
    public bool ducking = false;
    private bool running = false;
    private Vector2 defaultColliderSize;
    private Vector2 defaultColliderOffset;
    private Vector2 slideColliderOffset = new Vector2(0.06f,0.1f);
    private Vector2 slideColliderSize = new Vector2(0.32f,0.13f);
    private float defaultCheckWallDistance;
    private BoxCollider2D boxCollider2D;

    public bool queuedGetUp = false;

    private PhysicsMaterial2D physicsMaterial2D;
    private Coroutine StopBounceRoutine;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        checkGround = GetComponent<CheckGround>();
        checkWall = GetComponent<CheckWall>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        shootController = GetComponentInChildren<ShootController>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        defaultColliderSize = boxCollider2D.size;
        defaultColliderOffset = boxCollider2D.offset;
        defaultCheckWallDistance = checkWall.distanceToWall;

        inputGenerator.JumpEvent += Jump;
        inputGenerator.ChangeDirHorizontalEvent += BufferMovement;
        inputGenerator.ChangeDirVerticalEvent += CheckSlide;

        checkGround.bounceEvent += Bounce;
        checkGround.landedEvent += StopBounce;
        checkGround.landedEvent += CheckSlide;
       // checkWall.walledEvent += StopBounce;
        checkWall.walledEvent += SetWallDir;
        checkWall.walledEvent += StopSlide;

        physicsMaterial2D = rb.sharedMaterial;


    }   
    private void Start() {
        if(!CanGetUp())
            ApplySlide();
    } 

    private void FixedUpdate() {
        
        if(queuedGetUp && CanGetUp())
        {
            StopSlide();
            queuedGetUp = false;
        }
        if(sliding && !CanGetUp())
            ApplySmallSlide();

        if(!movementBlock)
        {
            if(!(checkGround.grounded && sliding ))
            {

                rb.velocity = new Vector2(bufferedMovementInput * speed, rb.velocity.y);
            }
        }
            
        if(!checkGround.grounded )
        {
//            Debug.Log("dsad");
            FastFall();
            if(sliding)
                CheckSlide(0);
        }
        
        if(!checkGround.grounded && checkWall.walled &&!bouncing)
        {
            WallSlide();
            if(sliding)
                StopSlide();
        }

        if(!checkGround.grounded && !checkWall.walled)
        {
            if(bufferedMovementInput == 1)
                SetRotation(0);
            if(bufferedMovementInput == -1)
                SetRotation(180);
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

            if(boxCollider2D.size == slideColliderSize && CanGetUp())
            {
                SetDefaultCollider();
            }
          

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
        if(sliding)
            StopSlide();
        if((checkGround.grounded || checkWall.walled))
        {
            Vector2 dir = jumpForce * Vector2.up;
            
           if(checkWall.walled)//wall jump
           {
                if(checkWall.isLeft)
                    SetRotation(0);
                else
                    SetRotation(180);
                dir = wallJumpForce * Vector2.up;
                checkWall.Sleep(0.1f);
                float dirMod = 1;
                if(!checkWall.isLeft)
                    dirMod = -1;
                dir += wallJumpHorizontalForce * Vector2.right * dirMod;

                BlockMovement();
                Invoke("UnblockMovement",wallJumpBlockMovementTime);
                rb.velocity = Vector2.zero;
                
                if(WallJumpEvent!=null)
                    WallJumpEvent.Invoke();
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
       /* if(checkWall.isLeft && bufferedMovementInput < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if(!checkWall.isLeft && bufferedMovementInput > 0)
        {
             rb.velocity = new Vector2(0, rb.velocity.y);
        }*/

        if(!checkWall.GoingDown())
        {
            return;
        }
        if(checkWall.isLeft)
            SetRotation(0);
        else
            SetRotation(180);
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
        Debug.Log("Bounced");
        if(inputGenerator.timeOffset != 0)
            return;

        if(BounceEvent!=null)
            BounceEvent.Invoke();

        ApplyBounce(bounciness,false);
        
        inputGenerator.SaveBounceInput(bounciness);
    }
    public void ApplyBounce(float bounciness, bool shouldBlockShoot)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce * bounciness ,ForceMode2D.Impulse);
        bouncing = true;
        if(shouldBlockShoot)
        {
            shootController.BlockShoot();

        }

        if(StopBounceRoutine != null)
            StopCoroutine(StopBounceRoutine);
        StopBounceRoutine = StartCoroutine(QueueStopBounce());
    }

    private IEnumerator QueueStopBounce()
    {
        yield return new WaitForSeconds(0.5f);
        StopBounce();
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
                ApplySlide();
            }
        }
        else if(dir == -1 && checkGround.grounded && bufferedMovementInput ==0)
        {
            SetSlideCollider();
            if(!ducking)
            {
                rb.velocity = Vector2.zero;
                ducking = true;
            }
        }
        else 
        {
            if(sliding && checkGround.grounded && CanGetUp())
            {
                StopSlide();
            }
            else if(!CanGetUp())
            {
                queuedGetUp = true;
                rb.AddForce(transform.right * slideForce /2,ForceMode2D.Impulse);
            }
            if(ducking)
                ducking = false;
        }
    }
    private void CheckSlide()
    {
        if(bufferedMovementInput == 1)
            SetRotation(0);
        if(bufferedMovementInput == -1)
            SetRotation(180);
        sliding = false;
        CheckSlide(bufferedVerticalInput);
    }

    private void StopSlide()
    {
        rb.sharedMaterial = physicsMaterial2D; 
        if(sliding && StopSlideEvent != null)
            StopSlideEvent.Invoke();
        SetDefaultCollider();
        sliding = false;
        ducking = false;
        if(bufferedMovementInput == 1)
            SetRotation(0);
        if(bufferedMovementInput == -1)
            SetRotation(180);
    }
    private void StopSlide(bool aux)
    {
        StopSlide();
    }
    private void ApplySlide()
    {
        
        rb.sharedMaterial = null;
        rb.AddForce(transform.right * slideForce ,ForceMode2D.Impulse);
        SetSlideCollider();
        sliding = true;
        if(SlideEvent!=null)
            SlideEvent.Invoke();
    }
    private void ApplySmallSlide()
    {
        
        rb.sharedMaterial = null;
        rb.AddForce(transform.right * slideForce /10,ForceMode2D.Impulse);
        SetSlideCollider();
        sliding = true;
        if(SlideEvent!=null)
            SlideEvent.Invoke();
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

    private void SetDefaultCollider()
    {
        boxCollider2D.size = defaultColliderSize;
        boxCollider2D.offset = defaultColliderOffset;

        checkWall.distanceToWall = defaultCheckWallDistance;
    }
    private void SetSlideCollider()
    {
        boxCollider2D.size = slideColliderSize;
        boxCollider2D.offset = slideColliderOffset;
        checkWall.distanceToWall = defaultCheckWallDistance * 2;
    }

    private bool CanGetUp()
    {
        RaycastHit2D hit;
        Vector3 pos = transform.position + Vector3.up* 0.2f;

        hit = Physics2D.Raycast(pos, Vector3.up, 0.3f,checkGround.groundLayers);
        Debug.DrawRay(pos, Vector3.up*0.3f, Color.red,1);
        if (hit.collider != null)
        {   
        Debug.Log(hit.collider.gameObject.name);
            return false;
        }
        
        return true;
    }

    private void SetWallDir(bool isLeft)
    {
        if(isLeft)
            SetRotation(0);
        else
            SetRotation(180);
    }
}
