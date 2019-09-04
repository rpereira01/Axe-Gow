using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected State _state = null;
    public Vector3 move;
    public int SpeedMultiplier;
    [SerializeField] private InputPlayerControls _controls;
    
    // Start is called before the first frame update
    void Awake(){
        _controls = new InputPlayerControls();
        _controls.Move.Move.performed += OnMove;
        _controls.Move.Move.canceled += ctx => move = Vector3.zero;
    }

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
        _controls.Move.Move.Enable();
    }

    void OnDisable(){
        _controls.Move.Move.performed -=  OnMove;
        _controls.Move.Move.Disable();
    }

    void OnMove (InputAction.CallbackContext context){
        move = new Vector3(context.ReadValue<Vector2>().x, 0 , context.ReadValue<Vector2>().y);
        SetState(new Walking(this));
    }
}
