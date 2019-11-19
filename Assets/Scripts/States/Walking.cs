using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : State 
{
    public Walking(PlayerController context)
    {
        SetContext(context);
    }
    public override void Tick()
    {
        // _context._animator.SetBool("isWalking", true);
        // _context._animator.SetFloat("xAxis", _context._direction.x);
        // _context._animator.SetFloat("zAxis", _context._direction.z);
    }
}