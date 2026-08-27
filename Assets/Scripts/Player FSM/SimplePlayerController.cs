using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ActionSkill
{
    None,
    Dash,
    Invis,
    Charge
}

public class SimplePlayerController: MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public ActionSkill myASkill;

    [Header("Speed and Acceleration")]
    public float speed;
    private Vector3 _velocity;
    private float _gravity = -9.18f;
    private bool _isGrounded;
    Vector3 _direction;
    private Vector3 _lastMovementDirection = Vector3.forward;
    private bool _isDashing;
    private Tween _dashTween;
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
    public InputAction actionSkill;
    public Slider aSkillCoolDownSlider;
    public float dashDistance = 2f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float dashCollisionBuffer = 0.05f;
    private float _dashCooldownTimer;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(readVector.y, 0, -readVector.x);
        _direction = toConvert;

        if (toConvert.sqrMagnitude > 0)
        {
            _lastMovementDirection = toConvert.normalized;
        }
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
        myASkill = (ActionSkill)Random.Range(1, 4);
        health = maxHealth;
    }
    void Start()
    {
        minusHP.Enable();
        plusHP.Enable();
        instantDeath.Enable();
        actionSkill.Enable();

        GameOverScreen.SetActive(false);

        if (aSkillCoolDownSlider != null)
        {
            aSkillCoolDownSlider.maxValue = dashCooldown;
            aSkillCoolDownSlider.value = 0f;
        }
    }

    void Update()
    {   
        _dashCooldownTimer = Mathf.Max(_dashCooldownTimer -= Time.deltaTime, 0f);
        if (aSkillCoolDownSlider != null)
        {
            aSkillCoolDownSlider.value = dashCooldown - _dashCooldownTimer;
        }

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

        if (actionSkill.WasPressedThisFrame())
        {
            Debug.Log("ASKill!");
            HandleActionSkill();
        }

        Movement();


        
    }

    void HandleActionSkill()
    {
        if (myASkill == ActionSkill.Dash && !_isDashing && _dashCooldownTimer <= 0f)
        {
            _isDashing = true;
            _dashCooldownTimer = dashCooldown;
            Vector3 dashTarget = GetDashTarget();
            float duration = Mathf.Max(dashDuration, 0.01f);

            _dashTween = transform.DOMove(dashTarget, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _isDashing = false;
                    _dashTween = null;
                });
        }
    }

    Vector3 GetDashTarget()
    {
        Vector3 direction = _lastMovementDirection.normalized;
        float allowedDistance = dashDistance;
        float radius = controller.radius;
        Vector3 center = transform.TransformPoint(controller.center);
        float halfHeight = Mathf.Max(controller.height * 0.5f - radius, 0f);
        Vector3 capsuleBottom = center + Vector3.down * halfHeight;
        Vector3 capsuleTop = center + Vector3.up * halfHeight;

        RaycastHit[] hits = Physics.CapsuleCastAll(
            capsuleBottom,
            capsuleTop,
            radius,
            direction,
            dashDistance,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponentInParent<SimplePlayerController>() == this ||
                hit.collider.GetComponentInParent<EnemyAI>() != null)
            {
                continue;
            }

            allowedDistance = Mathf.Min(
                allowedDistance,
                Mathf.Max(hit.distance - dashCollisionBuffer, 0f));
        }

        return transform.position + direction * allowedDistance;
    }

    void Movement()
    {
        AimWeaponAtMouse();
        if (_isDashing) return;

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

        Vector3 movement = _direction * speed * Time.deltaTime + _velocity;

        controller.Move(movement);
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
        if (_isDashing) return;

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
