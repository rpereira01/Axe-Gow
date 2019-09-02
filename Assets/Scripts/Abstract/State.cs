using System.Collections;
using System.Collections.Generic;

public abstract class State 
{
    protected PlayerController _context;

    public abstract void Tick();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public void SetContext (PlayerController context){
        this._context = context;
    }
}
