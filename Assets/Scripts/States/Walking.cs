using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : State 
{
    public Walking(PlayerController context)
    {
        SetContext(context);
    }
    public override void Tick(){
        Debug.Log("Walking");
    }
}