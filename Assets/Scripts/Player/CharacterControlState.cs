using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlState
{
    public bool sliding{get; set;}
    public bool bouncing{get; set;}
    public bool ducking{get; set;}
    public bool running{get; set;}
    public int animatorState{get;set;}
    public CharacterControlState()
    {
        
    }
    
}
