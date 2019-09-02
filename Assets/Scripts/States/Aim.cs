using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : State
{
    public Aim(PlayerController context)
    {
        SetContext(context);
    }

    public override void Tick(){
        Debug.Log("Aiming");
    }
}
