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
    public bool hasVelocity = false;
    public Vector2 velocity;
    public CharacterControlState characterControlState;
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
    public InputNode(float time, InputActionType actionType, float val, Vector3 pos,Vector2 velocity)
    {
        this.time = time;
        this.type = actionType;
        this.val = val;
        this.pos = pos;
        hasPos =true;
        this.velocity = velocity;
        hasVelocity= true;
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
    public InputNode(float time, InputActionType actionType, Vector3 pos,Vector2 velocity)
    {
        this.time = time;
        this.type = actionType;
        this.pos = pos;
        hasPos = true;
        this.velocity = velocity;
        hasVelocity = true;
    }
    public InputNode(float time, InputActionType actionType, float val,CharacterControlState characterControlState)
    {
        this.time = time;
        this.type = actionType;
        this.val = val;
        this.characterControlState = characterControlState;
    }
    public InputNode(float time, InputActionType actionType, float val, Vector3 pos,CharacterControlState characterControlState)
    {
        this.time = time;
        this.type = actionType;
        this.val = val;
        this.pos = pos;
        hasPos =true;
        this.characterControlState = characterControlState;
    }
    public InputNode(float time, InputActionType actionType, float val, Vector3 pos,Vector2 velocity,CharacterControlState characterControlState)
    {
        this.time = time;
        this.type = actionType;
        this.val = val;
        this.pos = pos;
        hasPos =true;
        this.velocity = velocity;
        hasVelocity= true;
        this.characterControlState = characterControlState;
    }
    public InputNode(float time, InputActionType actionType,CharacterControlState characterControlState)
    {
        this.time = time;
        this.type = actionType;
        this.characterControlState = characterControlState;
    }
    public InputNode(float time, InputActionType actionType, Vector3 pos,CharacterControlState characterControlState)
    {
        this.time = time;
        this.type = actionType;
        this.pos = pos;
        hasPos = true;
        this.characterControlState = characterControlState;
    }
    public InputNode(float time, InputActionType actionType, Vector3 pos,Vector2 velocity,CharacterControlState characterControlState)
    {
        this.time = time;
        this.type = actionType;
        this.pos = pos;
        hasPos = true;
        this.velocity = velocity;
        hasVelocity = true;
        this.characterControlState = characterControlState;
    }
}

public enum InputActionType
{
    Movement,
    Jump,
    JumpRelease,
    Shoot,
    Aim,
    Bounce
}