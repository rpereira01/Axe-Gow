using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State 
{
    public Idle(PlayerController context)
    {
        SetContext(context);
    }

    public override void Tick(){     
    }
}