using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 using UnityEngine.SceneManagement;
public class InputRead : InputGenerator
{
    [HideInInspector]public event UnityAction SlowTimeEvent;
    [HideInInspector]public event UnityAction RestoreTimeEvent;
    
    private Controls playerControls;

    void Awake()
    {
        playerControls = new Controls();
        playerControls.player.Jump.performed += _ => Jump();
        playerControls.player.JumpRelease.performed += _ => JumpRelease();
        playerControls.player.Shoot.performed += _ => Shoot();
        playerControls.player.Horizontal.performed += dir => BufferMovementHorizontal(dir.ReadValue<float>());
        playerControls.player.Vertical.performed += dir => BufferMovementVertical(dir.ReadValue<float>());
        playerControls.player.SlowTime.performed += _ => SlowTime();
        playerControls.player.RestoreTime.performed += _ => RestoreTime();
        playerControls.player.Restart.performed += _ => RestartScene();
    }

    private void OnEnable() 
    {
        playerControls.Enable();
    }

    private void OnDisable() 
    {
        playerControls.Disable();
    }

    protected override void Jump()
    {
        
        inputLog.AddAction(Time.time, InputActionType.Jump);
        RaiseJumpEvent();
    }
    protected override void Shoot()
    {
        inputLog.AddAction(Time.time, InputActionType.Shoot);
        RaiseShootEvent();
    }
    protected override void JumpRelease()
    {
        inputLog.AddAction(Time.time, InputActionType.JumpRelease);
        RaiseJumpReleaseEvent();
    }
    protected override void BufferMovementHorizontal(float dir)
    {
        inputLog.AddAction(Time.time, InputActionType.Movement, dir);
        RaiseChangeDirHorizontalEvent(dir);
    }
    protected override void BufferMovementVertical(float dir)
    {
        inputLog.AddAction(Time.time, InputActionType.Aim, dir);
        RaiseChangeDirVerticalEvent(dir);
    }

    private void SlowTime()
    {
        if(SlowTimeEvent!=null)
            SlowTimeEvent.Invoke();
    }
    private void RestoreTime()
    {
        if(RestoreTimeEvent!=null)
            RestoreTimeEvent.Invoke();
    }


    private void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
