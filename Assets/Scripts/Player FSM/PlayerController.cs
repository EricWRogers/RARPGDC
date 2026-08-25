using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("Speed and Acceleration")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float decceleration = 10f;
    private float _currentSpeed;
    private Vector3 _velocity;

    private float _gravity = -9.18f;
    private bool _isGrounded;
    Vector3 _direction;
    public CharacterController controller;

    // INPUT EVENTS
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(readVector.y, 0, -readVector.x);
        _direction = toConvert;
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        return;
    }

    public void OnInverse(InputAction.CallbackContext context)
    {
        return;
    }

    void Awake()
    {
        
    }
    void Start()
    {
         _currentSpeed = maxSpeed;
    }

    void Update()
    {   
        Movement();
        
    }

    void Movement()
    {
        //speed up and slow down
        if (_direction == Vector3.zero && _currentSpeed > 0)
        {
            _currentSpeed -= decceleration * Time.deltaTime;
        }
        else if (_direction != Vector3.zero && _currentSpeed < maxSpeed)
        {
            _currentSpeed += acceleration * Time.deltaTime;
        }

        //prevent speed from going past the max speed
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, maxSpeed);

        //gravity
        _isGrounded = controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2;
        }
        else
        {
            _velocity.y = _gravity * Time.deltaTime;
        }
        
        //put it all together
        controller.Move(_direction * _currentSpeed * Time.deltaTime + _velocity);
    }

        // //animation
        // if (_direction.z > 0)
        // {
        //     animator.Play("Walk_Left");
        // }
        // else if (_direction.z < 0)
        // {
        //     animator.Play("Walk_Right");
        // }
        // else if (_direction.x > 0)
        // {
        //     animator.Play("Walk_Up");
        // }
        // else if (_direction.x < 0)
        // {
        //     animator.Play("Walk_Down");
        // }
        // else
        // {
        //     animator.Play("Idle");
        // }
    public void Attack()
    {
        
    }
}
