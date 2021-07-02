using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSimulator : InputGenerator
{
    private float timePrecision = 0.1f;
    public float timeOffset = 1;
    private int actionIndex = 0;
    
    
    void FixedUpdate()
    {
        if(inputLog.inputs.Count <= actionIndex)return;

        float delay  = (Time.time - timeOffset) - inputLog.inputs[actionIndex].time;
        if(Mathf.Abs(delay) <= timePrecision)
        {
            StartCoroutine(QueueInputSim(inputLog.inputs[actionIndex].time , inputLog.inputs[actionIndex]));
            actionIndex++;
        }
    }

    private IEnumerator QueueInputSim(float desiredTime, InputNode node)
    {
        float delay;
        float startingOffset;
        startingOffset = timeOffset;
       /* do
        {*/
            delay = (Time.time - timeOffset) - desiredTime;
            startingOffset = timeOffset;
            yield return new WaitForSeconds(delay);
       // }while(startingOffset!=timeOffset);

        
        delay = Time.time - timeOffset - node.time;

       // print(Time.time -timeOffset - node.time);
        SimulateAction(node);
    }

    private void SimulateAction(InputNode node)
    {
            switch(node.type)
            {
                case InputActionType.Jump:
                    Jump();
                    break;
                case InputActionType.Movement:
                    BufferMovementHorizontal(node.val);
                    break;
                case InputActionType.JumpRelease:
                    JumpRelease();
                    break;
                case InputActionType.Shoot:
                    Shoot();
                    break;
                case InputActionType.Aim:
                    BufferMovementVertical(node.val);
                    break;
                default:
                    break;
            }
    }
    protected override void Jump()
    {
        RaiseJumpEvent();
    }
    protected override void Shoot()
    {
        RaiseShootEvent();
    }
    protected override void JumpRelease()
    {
        RaiseJumpReleaseEvent();
    }

    protected override void BufferMovementHorizontal(float dir)
    {
        RaiseChangeDirHorizontalEvent(dir);
    }
    protected override void BufferMovementVertical(float dir)
    {
        RaiseChangeDirVerticalEvent(dir);
    }
}
