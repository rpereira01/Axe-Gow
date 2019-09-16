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
    [Range(0, 50)] public float SpeedMultiplier;
    [Range(0, 50)] public float SpeedRotation;
    [Range(0,100)] public float ThrowPower;

    private Walking _walkingState;
    private Idle _idleState;
    private Aim _aimState;

    public GameObject mainCamera;
    public GameObject Axe;
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

        ControlsInitialization();
    }

    void Start()
    {
        this.SetState(_idleState);

    }

    // Update is called once per frame
    void Update()
    {
        _state.Tick();
    }
    void FixedUpdate()
    {
        MoveTo();
    }
    void LateUpdate()
    {        
        camDirection();
    }

    private void Rotation()
    {
        if (_rotation != Vector3.zero)
        {
            Vector3 rotation = _rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation), SpeedRotation);
        }
    }

    public void MoveTo()
    {  
        if(_direction != Vector3.zero){     
            Vector3 direction = _direction;           
            direction = direction.x * camRight + direction.z * camForward;
            transform.LookAt(transform.position + direction);   
            _rigidbody.MovePosition(transform.position + (direction * SpeedMultiplier * Time.fixedDeltaTime));   
        }     
    }

    void camDirection()
    {
        camRight = mainCamera.transform.right;
        camForward = mainCamera.transform.forward;

        camRight.y = 0;
        camForward.y = 0;
    }

    void SetState(State state)
    {
        _state = state;
    }

    public void AxeThrow()
    {
        Axe.GetComponent<Rigidbody>().isKinematic = false;
        Axe.transform.parent = null;
        Axe.GetComponent<Rigidbody>().AddForce(transform.forward * ThrowPower, ForceMode.Impulse);
    }

    void OnEnable()
    {
        _controls.Movement.Move.Enable();
        _controls.Movement.Rotate.Enable();
        _controls.Movement.Triggers.Enable();
    }

    void OnDisable()
    {
        _controls.Movement.Move.performed -= OnMove;
        _controls.Movement.Rotate.performed -= OnRotate;
        _controls.Movement.Triggers.performed -= OnTriggers;

        _controls.Movement.Move.Disable();
        _controls.Movement.Rotate.Disable();
        _controls.Movement.Triggers.Disable();
    }
    private void ControlsInitialization()
    {
        _controls.Movement.Move.performed += OnMove;
        _controls.Movement.Rotate.performed += OnRotate;
        _controls.Movement.Triggers.performed += OnTriggers;

        _controls.Movement.Move.canceled += ctx =>
        {
            _direction = Vector3.zero;
            SetState(_idleState);
        };

        _controls.Movement.Rotate.canceled += ctx => 
        {
            _rotation = Vector3.zero;    
        };

        _controls.Movement.Triggers.canceled += ctx =>
        {
            
        };
    }

    void OnMove(InputAction.CallbackContext context)
    {
        _direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        SetState(_walkingState);
    }

    void OnRotate(InputAction.CallbackContext context)
    {
        _rotation = new Vector3(0, context.ReadValue<Vector2>().x , 0);
    }

    void OnTriggers(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }
}
