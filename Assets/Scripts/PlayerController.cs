using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected State _state = null;
    [SerializeField] private InputPlayerControls _controls;
    
    // Start is called before the first frame update
    void Start()
    {
        
        this.SetState(new Idle(this));        
    }

    // Update is called once per frame
    void Update()
    {
        _state.Tick();
    }

    void SetState (State state){
        _state = state;
    }

    void OnEnable(){
        _controls = new InputPlayerControls();
        _controls.Move.Move.performed +=  OnMove;
        _controls.Move.Move.Enable();
    }

    void OnDisable(){
        _controls.Move.Move.performed -=  OnMove;
        _controls.Move.Move.Disable();
    }

    void OnMove (InputAction.CallbackContext context){
        Debug.Log(context);
        SetState(new Walking(this));
    }
}
