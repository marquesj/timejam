using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 [Serializable]
public class InputNode
{
    public InputActionType type;
    public float val = 0;
    public float time;
    public Vector3 pos;
    public bool hasPos = false;
    public InputNode(float time, InputActionType actionType, float val)
    {
        this.time = time;
        this.type = actionType;
        this.val = val;
    }
    public InputNode(float time, InputActionType actionType, float val, Vector3 pos)
    {
        this.time = time;
        this.type = actionType;
        this.val = val;
        this.pos = pos;
        hasPos =true;
    }
    public InputNode(float time, InputActionType actionType)
    {
        this.time = time;
        this.type = actionType;
    }
    public InputNode(float time, InputActionType actionType, Vector3 pos)
    {
        this.time = time;
        this.type = actionType;
        this.pos = pos;
        hasPos = true;
    }
}

public enum InputActionType
{
    Movement,
    Jump,
    JumpRelease,
    Shoot,
    Aim
}