using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected State _state = null;
    
    // Start is called before the first frame update
    void Start()
    {
        this.setState(new Idle(this));        
    }

    // Update is called once per frame
    void Update()
    {
        _state.Tick();
    }

    void setState (State state){
        _state = state;
        Debug.Log("This is my new state " + state);
    }
    
}
