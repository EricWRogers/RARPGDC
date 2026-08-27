using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ActionSkill
{
    None,
    Dash,
    Charge,
    Invis
    
}

public class SimplePlayerController: MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public ActionSkill myASkill;
    public RoomManager rm;

    [Header("Speed and Acceleration")]
    public float speed;
    private Vector3 _velocity;
    private float _gravity = -9.18f;
    private bool _isGrounded;
    Vector3 _direction;
    private Vector3 _lastMovementDirection = Vector3.forward;
    private bool _isDashing;
    private Tween _dashTween;
    private bool _isCharging;
    public bool _isInvis;
    private readonly HashSet<EnemyAI> _chargeEnemiesHit = new HashSet<EnemyAI>();
    public CharacterController controller;
    public bool aimingDisabled = false;

    [Header("Health")]
    public int maxHealth {get; private set;} = 30;
     public float health;
     public GameObject GameOverScreen;
    [Header("Combat")]
    public GameObject weapon;
    public Weapon weaponScript;
    [Header("Inversion")]
    public bool isNearWater;

    [Header("Appearance")]
    public SpriteRenderer sprite;


    // INPUT EVENTS //

    public InputAction minusHP;
    public InputAction plusHP;
    public InputAction instantDeath;
    public InputAction actionSkill;
    public InputAction resetLevel;
    public Slider aSkillCoolDownSlider;
    public float dashDistance = 2f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float dashCollisionBuffer = 0.05f;
    public float chargeDistance = 5f;
    public float chargeDuration = 0.4f;
    public float chargeCooldown = 1.5f;
    public float invisCooldown = 5.0f;
    public float invisTimer = 3.0f;
    private float invisIter = 0.0f;
    private float cooldownTimer;

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
        if (!context.performed) return;
        
        if (isNearWater)
        {
            GameObject _rm = GameObject.Find("RoomManager");
            RoomManager _rmS = _rm.GetComponent<RoomManager>();
            _rmS.Invert();
        }
        else return;
    }

    void Awake()
    {
        rm = FindFirstObjectByType<RoomManager>();
        myASkill = (ActionSkill)Random.Range(1, 4);
        health = maxHealth;
    }
    void Start()
    {
        minusHP.Enable();
        plusHP.Enable();
        instantDeath.Enable();
        actionSkill.Enable();
        resetLevel.Enable();

        GameOverScreen.SetActive(false);

        if (aSkillCoolDownSlider != null)
        {
            aSkillCoolDownSlider.maxValue = GetSkillCooldown();
            aSkillCoolDownSlider.value = 0f;
        }

        isNearWater = false;
    }

    void Update()
    {   
        cooldownTimer = Mathf.Max(cooldownTimer -= Time.deltaTime, 0f);
        if (aSkillCoolDownSlider != null)
        {
            aSkillCoolDownSlider.value = GetSkillCooldown() - cooldownTimer;
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
            HandleActionSkill();
        }

        if (resetLevel.WasPressedThisFrame())
        {
            SceneManager.LoadScene(0);
        }

        if (_isInvis)
        {
            if((invisIter += Time.deltaTime) >= invisTimer)
            {
                invisIter = 0.0f;
                _isInvis = false;
                GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }

        Movement();


        
    }

    void HandleActionSkill()
    {
        if (cooldownTimer > 0f || _isDashing || _isCharging || _isInvis) return;

        if (myASkill == ActionSkill.Dash)
        {
            StartDash();
        }
        else if (myASkill == ActionSkill.Charge)
        {
            StartCharge();
        }
        else if(myASkill == ActionSkill.Invis)
        {
            StartInvis();
        }

    }

    void StartDash()
    {
        _isDashing = true;
        cooldownTimer = dashCooldown;
        Vector3 dashTarget = GetSkillDest(dashDistance);

        _dashTween = transform.DOMove(dashTarget, Mathf.Max(dashDuration, 0.01f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _isDashing = false;
                _dashTween = null;
            });
    }

    void StartCharge()
    {
        _isCharging = true;
        cooldownTimer = chargeCooldown;
        _chargeEnemiesHit.Clear();
        Vector3 chargeTarget = GetSkillDest(chargeDistance);

        _dashTween = transform.DOMove(chargeTarget, Mathf.Max(chargeDuration, 0.01f))
            .SetEase(Ease.OutQuad)
            .OnUpdate(DamageEnemiesDuringCharge)
            .OnComplete(() =>
            {
                _isCharging = false;
                _dashTween = null;
            });
    }

    void StartInvis()
    {
        _isInvis = true;
        cooldownTimer = invisCooldown;

        GetComponentInChildren<SpriteRenderer>().enabled = false;

        // enemies know to stop looking when player is invis
    }

    float GetSkillCooldown()
    {
        //return myASkill == ActionSkill.Charge ? chargeCooldown : dashCooldown;
        switch (myASkill)
        {
            case ActionSkill.Dash:
                return dashCooldown;

            case ActionSkill.Charge:
                return chargeCooldown;

            case ActionSkill.Invis:
                return invisCooldown;

            default:
                return 0.0f;
        }

    }

    Vector3 GetSkillDest(float distance)
    {
        Vector3 direction = _lastMovementDirection.normalized;
        float allowedDistance = distance;
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
            distance,
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

    void DamageEnemiesDuringCharge()
    {
        Vector3 center = transform.TransformPoint(controller.center);
        float halfHeight = Mathf.Max(controller.height * 0.5f - controller.radius, 0f);
        Vector3 capsuleBottom = center + Vector3.down * halfHeight;
        Vector3 capsuleTop = center + Vector3.up * halfHeight;
        Collider[] contacts = Physics.OverlapCapsule(
            capsuleBottom,
            capsuleTop,
            controller.radius,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore);

        foreach (Collider contact in contacts)
        {
            EnemyAI enemy = contact.GetComponentInParent<EnemyAI>();
            if (enemy == null || !_chargeEnemiesHit.Add(enemy)) continue;

            enemy.TakeDamage(weaponScript.damage * 2);
        }
    }

    void Movement()
    {
        AimWeaponAtMouse();
        if (_isDashing || _isCharging) return;

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
        if (_isDashing || _isCharging) return;

        health -= damage;
    }
    public void Die()
    {
        speed = 0;
        sprite.color = Color.red;
        //reset game
        GameOverScreen.SetActive(true);
    }
}
