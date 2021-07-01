using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InputLog", menuName = "ScriptableObjects/InputLog")]
public class InputLog : ScriptableObject
{
    public List<InputNode> inputs;
    private void Awake() {
        inputs = new List<InputNode>();
    }
    private void OnEnable() {
        inputs = new List<InputNode>();
    }
    public void AddAction(float time, InputActionType action, float val)
    {
        inputs.Add(new InputNode(time, action, val));
    }
    public void AddAction(float time, InputActionType action)
    {
        inputs.Add(new InputNode(time, action));
    }
}


