using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSimulator : InputGenerator
{
    public TimeEvents timeEvents;
    private float timePrecision = 0.1f;

    private int actionIndex = 0;

    private float positionalErrorFixThreshold = 0.1f;
    private CharacterControl characterControl;
    private void Awake() {
        timeEvents.GoBackInTimeEvent += CancelCoroutines;
    }
    private void OnDestroy() {
        timeEvents.GoBackInTimeEvent -= CancelCoroutines;
    }
    void FixedUpdate()
    {
        if(inputLog.inputs.Count <= actionIndex)return;

        float delay  = (Time.time - timeOffset) - inputLog.inputs[actionIndex].time;
        if(Mathf.Abs(delay) <= timePrecision)
        {
            StartCoroutine(QueueInputSim(inputLog.inputs[actionIndex].time , inputLog.inputs[actionIndex], actionIndex));
            actionIndex++;
        }
        characterControl = GetComponent<CharacterControl>();
    }

    private IEnumerator QueueInputSim(float desiredTime, InputNode node, int nodeIndex)
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
        SimulateAction(node,nodeIndex);
    }

    private void SimulateAction(InputNode node, int nodeIndex)
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
                case InputActionType.Bounce:
                    Bounce(node.val);
                    break;
                default:
                    break;
            }

            if(node.hasPos)
            {
                Vector3 previousPos = transform.position;
                transform.position = node.pos;
                Vector3 positionalError = transform.position - previousPos;
                if(positionalError.magnitude > positionalErrorFixThreshold && nodeIndex != 0)
                {
                    // inputLog.inputs[nodeIndex-1].pos += positionalError;
                     
                    Debug.Log("Error");

                }
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

    private void Bounce(float force)
    {
        characterControl.ApplyBounce(force);
    }
    public override void SaveBounceInput(float force)
    {

    }

    private void CancelCoroutines(float aux)
    {
        StopAllCoroutines();
    }

}
