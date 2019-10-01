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
    [Range(0, 1)] public float SpeedRotation;
    [Range(0,100)] public float ThrowPower;
    [Range(0,300)] public float AxeRotationPower;
    public bool _isAiming;
    public bool _isThrowingAxe;
    public bool _axeThrown;
    public bool _canPull;

    private Walking _walkingState;
    private Idle _idleState;
    private Aim _aimState;
    private Throw _throwState;

    public GameObject MainCamera;
    public GameObject CineMachine;
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
        _throwState = new Throw(this);
        
        _controls = new InputPlayerControls();

        ControlsInitialization();
    }

    void Start()
    {
        this.SetState(_idleState);

    }

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
        if(_direction != Vector3.zero)
        {     
            Vector3 direction = _direction;           
            direction = direction.x * camRight + direction.z * camForward;
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), SpeedRotation);
            // _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), SpeedRotation));
            transform.LookAt(transform.position + direction);

            if(_state != _aimState)
            {
                _rigidbody.MovePosition(transform.position + (direction * SpeedMultiplier * Time.fixedDeltaTime));   
            }          
        }     
    }

    void camDirection()
    {
        camRight = MainCamera.transform.right;
        camForward = MainCamera.transform.forward;

        camRight.y = 0;
        camForward.y = 0;
    }

    void SetState(State state)
    {
        _state = state;
    }
    
    public void AxeThrow()
    {
        _axeThrown = true;
        Axe.GetComponent<Rigidbody>().isKinematic = false;
        Axe.transform.parent = null;
        Axe.GetComponent<Rigidbody>().AddForce(MainCamera.transform.TransformDirection(Vector3.forward) * ThrowPower, ForceMode.Impulse);
        Axe.GetComponent<Rigidbody>().AddTorque(Axe.transform.TransformDirection(Vector3.up) * AxeRotationPower, ForceMode.Impulse);
        _isThrowingAxe = false;
    }

    void OnEnable()
    {
        _controls.Movement.Move.Enable();
        _controls.Movement.Rotate.Enable();
        _controls.Movement.LeftTrigger.Enable();
        _controls.Movement.RightTrigger.Enable();
    }

    void OnDisable()
    {
        _controls.Movement.Move.performed -= OnMove;
        _controls.Movement.Rotate.performed -= OnRotate;
        _controls.Movement.LeftTrigger.performed -= OnTriggerLeft;
        _controls.Movement.RightTrigger.performed -= OnTriggerRight;

        _controls.Movement.Move.Disable();
        _controls.Movement.Rotate.Disable();
        _controls.Movement.LeftTrigger.Disable();
        _controls.Movement.RightTrigger.Disable();
    }

    private void ControlsInitialization()
    {
        _controls.Movement.Move.performed += OnMove;
        _controls.Movement.Rotate.performed += OnRotate;
        _controls.Movement.LeftTrigger.performed += OnTriggerLeft;
        _controls.Movement.RightTrigger.performed += OnTriggerRight;

        _controls.Movement.Move.canceled += ctx =>
        {
            _direction = Vector3.zero;
            SetState(_idleState);
        };

        _controls.Movement.Rotate.canceled += ctx => 
        {
            _rotation = Vector3.zero;    
        };

        _controls.Movement.LeftTrigger.canceled += ctx =>
        {
            _isAiming = false; 
            CineMachine.GetComponent<CinemachineFollowZoom>().enabled = false;
        };

        _controls.Movement.RightTrigger.canceled += ctx =>
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

    void OnTriggerLeft(InputAction.CallbackContext context)
    {
        if(context.action.activeControl.name.ToString() == "leftTrigger")
        {
            if(!_isAiming)
            {
                _isAiming = true;
                SetState(_aimState);
            }else{
                CameraZoom();
            }  
        }
    }    

    private void OnTriggerRight(InputAction.CallbackContext context)
    {
        if(context.action.activeControl.name.ToString() == "rightTrigger")
        {
            if(_isAiming)
            {
                if(!_axeThrown)
                {
                    _isThrowingAxe = true;
                    SetState(_throwState);
                } 

                if(_canPull)
                {
                    //SetState(_pullState);
                }               
            }           
        }
    }

    private void CameraZoom()
    {
        CineMachine.GetComponent<CinemachineFollowZoom>().enabled = true;
    }
}
