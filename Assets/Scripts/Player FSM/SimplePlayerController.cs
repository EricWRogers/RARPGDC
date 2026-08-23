using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SimplePlayer : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("Speed and Acceleration")]
    public float speed;
    private Vector3 _velocity;
    private float _gravity = -9.18f;
    private bool _isGrounded;
    Vector3 _direction;
    public CharacterController controller;

    [Header("Health")]
    public int maxHealth;
    [SerializeField] private int health;
    [Header("Combat")]
    public GameObject weapon;
    public Weapon weaponScript;


    // INPUT EVENTS //

    public InputAction minusHP;
    public InputAction plusHP;
    public InputAction instantDeath;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(readVector.y, 0, -readVector.x);
        _direction = toConvert;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        weaponScript.Attack();
    }

    void Awake()
    {
        health = maxHealth;
    }
    void Start()
    {

    }

    void Update()
    {   
        //cheat keybinds
        if (minusHP.WasPressedThisFrame())
        {
            Debug.Log("minus health");
            health --;
        }
        if (plusHP.WasPressedThisFrame())
        {
            Debug.Log("plus health");
            health ++;
        }
        if (instantDeath.WasPressedThisFrame())
        {
            Debug.Log("player killed");
            health = 0;
        }

        Movement();
        
    }

    void Movement()
    {
        //animation
        if (_direction.z > 0)
        {
            weapon.transform.rotation = Quaternion.Euler(0, -180, 0);
            Debug.Log("Left??");
        }
        if (_direction.z < 0)
        {
            weapon.transform.rotation = Quaternion.Euler(0, -0, 0);
        }

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
        controller.Move(_direction * speed * Time.deltaTime + _velocity);
    }

    

    //stretch goal
    public void Dodge()
    {
        
    }

    public void Die()
    {
        
    }
}
