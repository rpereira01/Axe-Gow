using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : State
{
    public Throw(PlayerController context)
    {
        SetContext(context);
    }

    public override void Tick()
    {
        if(_context._axeThrown)
        {
            _context._isThrowingAxe = false;
        }else{
            _context._animator.SetBool("isThrowing", _context._isThrowingAxe);
        }
    }
}
