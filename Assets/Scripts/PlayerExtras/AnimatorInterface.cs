using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class AnimatorInterface : MonoBehaviour

{
    private CharacterControl characterControl;
    private ShootController shootController;
    private Animator animator;
    private float shotAnimTime = 1f;
    private Coroutine restoreShotRoutine;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        characterControl = transform.parent.GetComponent<CharacterControl>();
        shootController = transform.parent.GetComponent<ShootController>();

    }
    private void Start() {
        
        characterControl.StopRunningEvent += StopWalkingEvent;
        characterControl.StartRunningEvent += StartWalkingEvent;
        shootController.ShootEvent += ShootEvent;
        characterControl.JumpEvent += JumpEvent;
        characterControl.checkGround.landedEvent += LandEvent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetBool("Grounded", characterControl.checkGround.grounded);
        animator.SetBool("GoingUp", characterControl.rb.velocity.y > 0);
        animator.SetFloat("AimDir", characterControl.bufferedVerticalInput);
        animator.SetBool("AimingNeutral",  characterControl.bufferedVerticalInput == 0);

    }

    private void JumpEvent()
    {
        animator.SetTrigger("Jump");
    }
    private void ShootEvent()
    {
        animator.SetBool("Shooting",  true);
        if(restoreShotRoutine != null)
            StopCoroutine(restoreShotRoutine);
        restoreShotRoutine = StartCoroutine(RestoreShootingBool());
    }

    private IEnumerator RestoreShootingBool()
    {
        yield return new WaitForSeconds(shotAnimTime);
        animator.SetBool("Shooting",  false);
    }

    private void DoubleJumpEvent()
    {
        animator.SetTrigger("Jump");
    }
    private void WallJumpEvent()
    {
        animator.SetTrigger("Jump");
    }
    private void LandEvent() 
    {
       animator.SetTrigger("Land");
    }

    private void FloatEvent()
    {
        animator.SetTrigger("Float");
    }

    private void ClingEvent()
    {
        animator.SetTrigger("Cling");
    }

    private void StartWalkingEvent()
    {
        animator.SetBool("Running", true);
    }
    private void StopWalkingEvent()
    {
        animator.SetBool("Running", false);
    }
}
