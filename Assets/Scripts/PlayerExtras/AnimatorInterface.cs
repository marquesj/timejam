using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimatorInterface : MonoBehaviour

{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void JumpEvent()
    {
        animator.SetTrigger("Jump");
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
