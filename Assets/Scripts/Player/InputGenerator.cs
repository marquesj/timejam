using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class InputGenerator : MonoBehaviour
{
    public InputLog inputLog;
    public float timeOffset = 1;
    protected float timeSinceAwake;
    [HideInInspector]public event UnityAction JumpEvent;
    [HideInInspector]public event UnityAction<float> ChangeDirHorizontalEvent;
    [HideInInspector]public event UnityAction ShootEvent;
    [HideInInspector]public event UnityAction<float> ChangeDirVerticalEvent;
    [HideInInspector]public bool jumpHeld = false;
    protected abstract void Jump();
    protected abstract void Shoot();
    protected abstract void JumpRelease();
    protected abstract void BufferMovementHorizontal(float dir);
    protected abstract void BufferMovementVertical(float dir);
    public abstract void SaveBounceInput(float force);

    protected void RaiseJumpEvent()
    {
        jumpHeld = true;
        if(JumpEvent!=null)
            JumpEvent.Invoke();
    }
    protected void RaiseJumpReleaseEvent()
    {
        jumpHeld = false;
    }

    protected void RaiseChangeDirHorizontalEvent(float dir)
    {
        if(ChangeDirHorizontalEvent!=null)
            ChangeDirHorizontalEvent.Invoke(dir);
    }

    protected void RaiseShootEvent()
    {

        if(ShootEvent!=null)
            ShootEvent.Invoke();
    }
    protected void RaiseChangeDirVerticalEvent(float dir)
    {
        if(ChangeDirVerticalEvent!=null)
            ChangeDirVerticalEvent.Invoke(dir);
    }
}
