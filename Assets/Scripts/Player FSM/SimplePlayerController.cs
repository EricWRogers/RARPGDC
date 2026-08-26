using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerController: MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("Speed and Acceleration")]
    public float speed;
    private Vector3 _velocity;
    private float _gravity = -9.18f;
    private bool _isGrounded;
    Vector3 _direction;
    public CharacterController controller;
    public bool aimingDisabled = false;

    [Header("Health")]
    public int maxHealth {get; private set;} = 30;
     public float health;
     public GameObject GameOverScreen;
    [Header("Combat")]
    public GameObject weapon;
    public Weapon weaponScript;
    [Header("Appearance")]
    public SpriteRenderer sprite;


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
        if (!context.performed) return;

        if (weapon != null && weaponScript != null)
        {
            weaponScript.Attack();
        }
        else
        {
            return;
        }
        
    }
    public void OnInverse(InputAction.CallbackContext context)
    {
        GameObject _rm = GameObject.Find("RoomManager");
        RoomManager _rmS = _rm.GetComponent<RoomManager>();
        _rmS.Invert();
    }

    void Awake()
    {
        health = maxHealth;
    }
    void Start()
    {
        minusHP.Enable();
        plusHP.Enable();
        instantDeath.Enable();

        GameOverScreen.SetActive(false);
    }

    void Update()
    {   
        if (health <= 0)
        {
            Die();
        }

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
        AimWeaponAtMouse();

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

    void AimWeaponAtMouse()
    {
        if (weapon == null || Camera.main == null || aimingDisabled) return;

        Ray mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        if (!playerPlane.Raycast(mouseRay, out float distance)) return;

        Vector3 mouseWorldPosition = mouseRay.GetPoint(distance);
        Vector3 aimDirection = mouseWorldPosition - transform.position;
        aimDirection.y = 0;

        if (aimDirection.sqrMagnitude > 0)
        {
            float angle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg + 180f;
            weapon.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    //stretch goal
    public void Dodge()
    {
        
    }

    public void Die()
    {
        speed = 0;
        sprite.color = Color.red;
        //reset game
        GameOverScreen.SetActive(true);
    }
}
