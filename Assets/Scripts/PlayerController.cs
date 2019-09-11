using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    protected State _state = null;
    private Vector3 camRight;
    private Vector3 camForward;
    public Vector3 _direction;
    public Vector3 _rotation;
    public int SpeedMultiplier;

    private Walking _walkingState;
    private Idle _idleState;
    private Aim _aimState;

    public GameObject mainCamera;
    [SerializeField] private InputPlayerControls _controls;
    [HideInInspector] public Rigidbody _rigidbody;
    [HideInInspector] public Animator _animator;
    

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _idleState = new Idle(this);
        _aimState = new Aim(this);
        _walkingState = new Walking(this);
        
        _controls = new InputPlayerControls();
        _controls.Movement.Move.performed += OnMove;
        _controls.Movement.Move.canceled += ctx => 
            {
                _direction = Vector3.zero;
                SetState(_idleState);                
            };
        _controls.Movement.Rotate.performed += OnRotate;
        _controls.Movement.Rotate.canceled += ctx => _rotation = Vector3.zero;
    }

    void Start()
    {        
        this.SetState(_idleState);        
    }

    // Update is called once per frame
    void Update()
    {
        _state.Tick();
        camDirection();
        RotateTo(_rotation);       
    }

    void FixedUpdate()
    {
        MoveTo(_direction);        
    }

    public void MoveTo(Vector3 direction)
    {   direction = direction.x * camRight + direction.z * camForward;
        gameObject.transform.LookAt(gameObject.transform.position + direction);
        _rigidbody.MovePosition(gameObject.transform.position + (direction * SpeedMultiplier * Time.fixedDeltaTime));
    }

    public void RotateTo(Vector3 rotation)
    {
        transform.Rotate(rotation * 150f * Time.deltaTime, Space.World);
        // transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, transform.localEulerAngles.z);        
    }

    void camDirection(){
        camRight = mainCamera.transform.right;
        camForward = mainCamera.transform.forward;

        camRight.y = 0;
        camForward.y = 0;

        camRight = camRight.normalized;
        camForward = camForward.normalized;
    }

    void SetState (State state){
        _state = state;
    }

    void OnEnable()
    {                
        _controls.Movement.Move.Enable();
        _controls.Movement.Rotate.Enable();
    }

    void OnDisable()
    {
        _controls.Movement.Move.performed -=  OnMove;
        _controls.Movement.Rotate.performed -= OnRotate;
        _controls.Movement.Move.Disable();
        _controls.Movement.Rotate.Disable();
    }

    void OnMove (InputAction.CallbackContext context)
    {
        _direction = new Vector3(context.ReadValue<Vector2>().x, 0 , context.ReadValue<Vector2>().y);
        SetState(_walkingState);
    }

    void OnRotate (InputAction.CallbackContext context)
    {
        _rotation = new Vector3(0, context.ReadValue<Vector2>().x , 0);
    }
}
