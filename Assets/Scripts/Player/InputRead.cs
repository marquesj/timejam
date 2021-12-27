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
    private float currentHorizontal = 0;
    private float currentVertical = 0;
    public  GameObject pauseMenu;
    

    protected override void Awake()
    {
        base.Awake();
        playerControls = new Controls();
        playerControls.player.Horizontal.performed += dir => BufferMovementHorizontal(dir.ReadValue<float>());
        playerControls.player.Vertical.performed += dir => BufferMovementVertical(dir.ReadValue<float>());
        playerControls.player.Jump.performed += _ => Jump();
        playerControls.player.JumpRelease.performed += _ => JumpRelease();
        playerControls.player.Shoot.performed += _ => Shoot();
        playerControls.player.SlowTime.performed += _ => SlowTime();
        playerControls.player.RestoreTime.performed += _ => RestoreTime();
        playerControls.player.Restart.performed += _ => RestartScene();
        playerControls.player.Pause.performed += _ => PauseScene();
        timeOffset = 0;
    }

    void Start()
    {

        GameObject[] objs= GameObject.FindGameObjectsWithTag("Pause");
        if(objs != null)
            pauseMenu = objs[0];

    }

    private void OnEnable() 
    {
        
        playerControls.Enable();
        BufferMovementHorizontal(playerControls.player.Horizontal.ReadValue<float>());
        BufferMovementVertical(playerControls.player.Vertical.ReadValue<float>());

    }
    private void  SafetyThing()
    {
        BufferMovementHorizontal(0);
        BufferMovementVertical(0);
        playerControls.Enable();
        BufferMovementHorizontal(0);
        BufferMovementVertical(0);
    }

    private void OnDisable() 
    {
        playerControls.Disable();
                BufferMovementHorizontal(0);
        BufferMovementVertical(0);
    }

    protected override void Jump()
    {
        
        inputLog.AddAction(Time.time, InputActionType.Jump, transform.position,rb.velocity,characterControl.GetState());
        RaiseJumpEvent();
    }
    protected override void Shoot()
    {
        inputLog.AddAction(Time.time, InputActionType.Shoot, transform.position,rb.velocity,characterControl.GetState());
        RaiseShootEvent();
    }
    protected override void JumpRelease()
    {
        inputLog.AddAction(Time.time, InputActionType.JumpRelease, transform.position,rb.velocity,characterControl.GetState());
        RaiseJumpReleaseEvent();
    }
    protected override void BufferMovementHorizontal(float dir)
    {
        
        if(dir >0)
            dir = 1;
        if(dir < 0)
            dir = -1;

        if(dir == currentHorizontal)
            return;

        currentHorizontal = dir;

        inputLog.AddAction(Time.time, InputActionType.Movement, dir, transform.position,rb.velocity,characterControl.GetState());
        RaiseChangeDirHorizontalEvent(dir);
    }
    protected override void BufferMovementVertical(float dir)
    {
        if(dir >0)
            dir = 1;
        if(dir < 0)
            dir = -1;

        if(dir == currentVertical)
            return;

        currentVertical = dir;

        inputLog.AddAction(Time.time, InputActionType.Aim, dir, transform.position,rb.velocity,characterControl.GetState());
        RaiseChangeDirVerticalEvent(dir);
    }

    public override void SaveBounceInput(float force)
    {
        inputLog.AddAction(Time.time, InputActionType.Bounce, force, transform.position,rb.velocity,characterControl.GetState());
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
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    private void PauseScene()
    {
        pauseMenu.GetComponentInChildren<Pausemenu>().Paused();
    }
}
