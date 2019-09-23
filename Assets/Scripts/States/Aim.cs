using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : State
{
    public Aim(PlayerController context)
    {
        SetContext(context);
    }

    public override void Tick()
    {
        cameraZoom();
        if(_context._isAiming) 
        {
            _context._animator.SetBool("isAiming",true);            
        }
    }

    private void cameraZoom()
    {
        //zoom;
    }
}
