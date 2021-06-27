using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CheckGround))]
[RequireComponent(typeof(CheckWall))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GamepadRumble))]

public class CharacterController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public CheckGround checkGround;
    public CheckGround checkBouncyGround;
    [Header("Movement Parameters")]
    public float speed = 1;
    public float arialSpeedMod = 1;
    public float turnArroundInAirSpeedMod = 1;

    public float airAcceleration = 1;

    [Header("Jump Parameters")]
    public float jumpForce = 1;
    public float fallMultiplier = 1;
    public float lowJumpMultiplier = 1;
    public float horizontalSpeedModifier = 1;
    public float bounceForce = 1;
    [Header("Double Jump Parameters")]
    public float secondJumpModifier = 1;
    public float doubleJumpFall = 1;
    public float doubleJumpLow = 1;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Vector2 projectileSpawnOffset = Vector2.zero;
    public float projectileForceMultiplier = 1;
    public float blockedMovementTimeAfterProject = 1;
    public float ShotCooldown = 1;

    public float shootGravityMod =1;

    public int maxProjectilesOnScene = 0;
    public float maxFloatTime = 2;


    [Header("Wall Jump")]
    public float slideDownWallMinSpeed = 1;
    public float slideDownWallMaxSpeed = 1;
    public float slideAccelerationTime = 1;
    public float slipperyAccelarationTime = 1;
    public float slipperyMaxSpeed = 1f;
    public float blockTimeAfterWallJump = .5f;
    public Vector2 wallJumpDir = new Vector2(.5f,.5f);
    public float wallJumpForce = 1;
    private Controls playerControls;
    private Rigidbody2D rb;
    private GamepadRumble gamepadRumble;


    private CheckWall checkWall;


    private float bufferedMovementInput;
    private float bufferedImpulse;
    private bool hasTurnedInAir = false;
    private bool jumpHeld ;
    public bool hasDoubleJump = true;
    public bool useDoubleJumpPhysics = false;
    public bool bouncing = false;

    private bool canShoot = true;

    private bool movementBlock = false;
    private float graviyMod = 1;

    private bool shootHeld;
    private float slideDownWallSpeed;
    private bool walking = false;

    private Coroutine clingRoutine = null;
    private Coroutine queuedStopFloat = null;
    private Coroutine queuedUnblockMovement = null;
    private Coroutine queuedReactToProjectileDeath = null;
    private int projectileCount = 0;
    private int airShootsCount = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gamepadRumble = GetComponent<GamepadRumble>();

        //checkGround = GetComponent<CheckGround>();
        checkWall = GetComponent<CheckWall>();

        playerControls = new Controls();
        checkWall.playerControls = playerControls;
        checkWall.checkGround = checkGround;

        playerControls.player.Jump.performed += _ => Jump();

        playerControls.player.Horizontal.performed += input => BufferMovement(input.ReadValue<float>());
        playerControls.player.Shoot.performed += _ => Shoot();

        checkGround.landedEvent += Land;
        if(checkBouncyGround!=null)
            checkBouncyGround.landedEvent += BounceLand;
        checkWall.walledEvent += Cling;
        
     //   jumpHeld =  playerControls.Player.Jump.ReadValue<bool>();
      //  shootHeld =  playerControls.Player.Shoot.ReadValue<bool>();
    }

    private void OnEnable() 
    {
        playerControls.Enable();
        bufferedMovementInput = 0;
        
      ///  jumpHeld =  playerControls.Player.Jump.ReadValue<bool>();
    //    shootHeld =  playerControls.Player.Shoot.ReadValue<bool>();
    }

    private void OnDisable() 
    {
        playerControls.Disable();
    }

    // Update is called once per frame

    private void FixedUpdate() 
    {
        HorizontalMovementUpdate();

        if(!checkGround.grounded)
        {
            if(checkWall.walled)
                ClingToWallUpdate();
            else
                FastFall();
        }
    }

    private void HorizontalMovementUpdate()
    {
        if(checkGround.grounded)
        {
            if(!movementBlock)
            {

                Move(bufferedMovementInput);
                FlipIfNeeded(bufferedMovementInput);
                ManageWalkEvent(bufferedMovementInput);
            }
        }
        else
        {
            AerialHorizontalMovement();
        }
    }

    private void ManageWalkEvent(float bufferedInput)
    {
        if(Mathf.Abs(bufferedInput) > 0.01f)
        {
            if(!walking)
            {
                walking = true;
               // transform.BroadcastMessage("StartWalkingEvent");
            }
        }else
        {
            if(walking)
            {
                walking = false;
                //transform.BroadcastMessage("StopWalkingEvent");
            }
        }
    }

    private void AerialHorizontalMovement()
    {
        float mov;
        if(Mathf.Sign(bufferedMovementInput) != Mathf.Sign(bufferedImpulse) &&  bufferedImpulse != 0 && bufferedMovementInput != 0)
        {
            bufferedImpulse = 0;
            hasTurnedInAir = true;
        }
        if(hasTurnedInAir)
            mov = bufferedMovementInput*turnArroundInAirSpeedMod;
        else
            mov = bufferedMovementInput;
        if(!movementBlock)
            AerialMove(mov,bufferedImpulse);
    }

    private void BufferMovement(float dir)
    {
        bufferedMovementInput = dir;
    }

    private void Jump()
    {
        
         if(movementBlock)return;
        if(checkWall.walled && !checkGround.grounded && checkWall.isJumpable)
        {
            //walljump
            BlockMovement();
            checkWall.Sleep(blockTimeAfterWallJump);
            Vector2 wallJumpVec = wallJumpDir*wallJumpForce;
            if(!checkWall.isLeft)
                wallJumpVec = new Vector2(-wallJumpVec.x,wallJumpVec.y);
            rb.AddForce(wallJumpVec, ForceMode2D.Impulse);
            Invoke("UnblockMovement", blockTimeAfterWallJump);
           // transform.BroadcastMessage("WallJumpEvent");
        }
        else if((checkGround.grounded /*|| hasDoubleJump */ ))
        {
            
            Vector2 dir = new Vector2(bufferedMovementInput*horizontalSpeedModifier,1);
            if(!checkGround.grounded )
            {
 
                rb.velocity = new Vector2(rb.velocity.x,0);
                dir *= secondJumpModifier;
                hasDoubleJump = false;
                useDoubleJumpPhysics = true;
                bouncing  =false;
              //  transform.BroadcastMessage("DoubleJumpEvent");
            }else
            {
               // transform.BroadcastMessage("JumpEvent");
            }
            dir *= jumpForce;
            rb.AddForce(dir ,ForceMode2D.Impulse);
            bufferedImpulse = bufferedMovementInput*speed*horizontalSpeedModifier;
            hasTurnedInAir = false;

        }
        jumpHeld = true;
    }

    private void JumpRelease()
    {
        jumpHeld = false;
    }

    public void Move(float dir)
    {
        rb.velocity = new Vector2(dir*speed,rb.velocity.y);
    }

    public void AerialMove(float dir, float impulse)
    {

        rb.AddForce(new Vector2(dir*airAcceleration,0), ForceMode2D.Force);
        if(Mathf.Abs(rb.velocity.x) > speed*arialSpeedMod )
            rb.velocity = new Vector2(impulse + dir*speed*arialSpeedMod,rb.velocity.y);
    }

    public void FlipIfNeeded(float dir)
    {
        if(spriteRenderer == null || Mathf.Abs(dir) < 0.03f)return;
        if(dir < 0 && spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
          //  transform.BroadcastMessage("FlipEvent",-1);
        }
        else if(dir > 0  && spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
         //   transform.BroadcastMessage("FlipEvent",1);
        }
    }

    private void FastFall()
    {
        float fallMult = fallMultiplier;
        float lowJump = lowJumpMultiplier;
        if(useDoubleJumpPhysics)
        {
            fallMult = doubleJumpFall;
            lowJump  = doubleJumpLow;
        }
        if(bouncing)
        {

            lowJump = 1;
        }
        if(rb.velocity.y < 0 )
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime ;
        }else if(rb.velocity.y > 0 && !jumpHeld)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJump - 1) * Time.deltaTime ;
        }
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * graviyMod);
    }
    private void Land()
    {
       // Debug.Log("landed");
      //  transform.BroadcastMessage("LandEvent");
      /*  if(walking)
            transform.BroadcastMessage("StartWalkingEvent");*/
        RestoreLandProperties();
    }
    private void BounceLand()
    {
      //  transform.BroadcastMessage("JumpEvent");
        RestoreLandProperties();
        bouncing = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * bounceForce ,ForceMode2D.Impulse);
    }
    private void Cling(bool isLeft)
    {
        
        RestoreLandProperties();
        bufferedMovementInput = playerControls.player.Horizontal.ReadValue<float>();
      /*  if(movementBlock)
            UnblockMovement();
        if(queuedUnblockMovement != null)
            StopCoroutine(queuedUnblockMovement);*/
        if(!isLeft)
        {
            FlipIfNeeded(-1);
        }else
        {

            FlipIfNeeded(1);
        }
        //transform.BroadcastMessage("ClingEvent");

      //  if(rb.velocity.y > 0)return;
        rb.velocity = Vector2.zero;
        slideDownWallSpeed = 0;
        if(clingRoutine != null)
            StopCoroutine(clingRoutine);

        float accTime = slideAccelerationTime;
        if(!checkWall.isJumpable)
            accTime = slipperyAccelarationTime;
        clingRoutine = StartCoroutine(ChangeSlideSpeed(accTime));

    }

    private void RestoreLandProperties()
    {
        canShoot = true;
        bouncing = false;
        useDoubleJumpPhysics = false;
        hasDoubleJump = true;
        airShootsCount = 0; 
    }

    private void ClingToWallUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x ,-slideDownWallSpeed);
    }

    private IEnumerator ChangeSlideSpeed(float duration)
    {
        float startTime = Time.time;
        float percent = 0;
        float maxSpeed;
        slideDownWallSpeed = slideDownWallMinSpeed;

        if(checkWall.isJumpable)
            maxSpeed = slideDownWallMaxSpeed;
        else
        {
                maxSpeed = slipperyMaxSpeed;
        }

        while(percent < 1)
        {
            percent = (Time.time - startTime)/duration;
            slideDownWallSpeed = Mathf.SmoothStep(slideDownWallMinSpeed,maxSpeed,percent*percent);
            yield return null;
        }

    }

    private void Shoot()
    {
        shootHeld = true;

        if(!canShoot)return;

        if(!checkGround.grounded)
        {
            
            airShootsCount++;
            if(airShootsCount > maxProjectilesOnScene)
                return;
          //  transform.BroadcastMessage("FloatEvent");
        }

        Vector3 offset = new Vector3(projectileSpawnOffset.x, projectileSpawnOffset.y , 0);
        GameObject projectile = Instantiate(projectilePrefab, transform.position + offset , Quaternion.identity);

        canShoot = false;


      //  graviyMod = shootGravityMod;
      //  rb.gravityScale = shootGravityMod;
      //  BlockMovement();
        Invoke("RestoreShot",ShotCooldown);
    }

    public void Project((Vector2,float) properties)
    {
        /*if(queuedReactToProjectileDeath != null)
            StopCoroutine(queuedReactToProjectileDeath);*/
        useDoubleJumpPhysics = true;
        Vector2 dir = properties.Item1.normalized;
        float velocity = properties.Item2;
        //canShoot = false;
        BlockMovement();
        rb.AddForce(dir*velocity*projectileForceMultiplier, ForceMode2D.Impulse);

        queuedUnblockMovement = StartCoroutine(QueueUnblockMovement( blockedMovementTimeAfterProject));
 
        FlipIfNeeded(dir.x);
        gamepadRumble.StartHaptics();
    }



    private IEnumerator QueueUnblockMovement(float time)
    {
        yield return new WaitForSeconds(time);
        UnblockMovement();
    }


    private void BlockMovement()
    {
        if(queuedUnblockMovement != null)
            StopCoroutine(queuedUnblockMovement);
       // transform.BroadcastMessage("StopWalkingEvent");
        movementBlock = true;
        rb.velocity = Vector2.zero;
    }


    private void UnblockMovement()
    {
        movementBlock = false;
    }

    private void RestoreShot()
    {
        canShoot = true;

    }
}
