using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InputLog", menuName = "ScriptableObjects/InputLog")]
public class InputLog : ScriptableObject
{
    [SerializeField]public List<InputNode> inputs;
    private void Awake() {
        inputs = new List<InputNode>();
    }
    private void OnEnable() {
        inputs = new List<InputNode>();
    }
    public void AddAction(float time, InputActionType action, float val)
    {
        if(Time.timeScale == 0)return;
        inputs.Add(new InputNode(time, action, val));
    }
    public void AddAction(float time, InputActionType action)
    {
        if(Time.timeScale == 0)return;
        inputs.Add(new InputNode(time, action));
    }

    public void RevertTo(float time)
    {
      
        for(int i = inputs.Count-1; i >= 0; i--)
        {
            if(inputs[i].time > time)
                inputs.RemoveAt(i);
            else
                inputs[i].time += (Time.time - time);
        }

       
    }
}


