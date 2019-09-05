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
        // Vector3 m = new Vector3(_context.move.x, _context.move.y, _context.move.z) * Time.deltaTime * _context.SpeedMultiplier;
        // _context.transform.Translate(m,Space.World);
        //Set Animation of Walking
    }
}