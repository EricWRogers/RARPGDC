using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection settings")]
    public Transform player { get; private set; }
    public float sightRange = 8f;     
    public LayerMask obstacleMask;    
    [SerializeField] private float eyeHeight = 1.5f;

    [Header("Attack settings")]
    public float attackRange = 2.0f;
    [Range(-1f,1f)]
    public float attackCone = 0.5f;

    [Header("Movement settings")]
    public float moveSpeed = 2f;      
    public float chaseSpeed = 4f;     

    [Header("Health")]
    public int maxhealth;
    public int health;
    bool isDying;
    [Header("Inverse")]
    public bool isInversed;
    public Collider col;

    public Vector3 LastKnownPosition { get; set; }
    public ChaseState Chase { get; private set; }
    public SearchState Search { get; private set; }
    public AttackState Attack { get; private set; }
    public WaitState Wait { get; private set; }
    public StunState Stun { get; private set; }
    public UnityEngine.AI.NavMeshAgent Agent { get; private set; }
    private MonsterIState currentState;

    public GameObject playerGO;

    [Header("Stun Settings")]
    public float stunTimer;
    

    void Awake()
    {
        maxhealth = Random.Range(6, 9);
        health = maxhealth;

        playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
        }
        else
        {
            Debug.LogError($"WHERE IS THE PLAYER??? {gameObject.name} CANT FIND ANYTHING TAGGED PLAYER!");
        }

        //randomly decides if monster is inversed or not
        isInversed = Random.value < 0.5f;

        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        Wait = new WaitState();
        Search = new SearchState();
        Chase = new ChaseState();
        Attack = new AttackState();
        Stun = new StunState();
    }

    void Start() 
    {
        SetInversion();
        ChangeState(Wait); 
    }

    void Update() 
    {         
        currentState?.OnUpdate(); 
    }

    public void ChangeState(MonsterIState nextState)
    {
        currentState?.OnExit();
        currentState = nextState;
        currentState.OnEnter(this);
    }

    void OnDrawGizmos()
    {
        if (player == null)
        {
            return;
        }

        Gizmos.DrawRay(transform.position + Vector3.up * eyeHeight, (player.position - (transform.position + Vector3.up * eyeHeight)).normalized);
    }

    public bool CanSeePlayer()
    {
        if (player == null)
        {
            return false;
        }

        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        

        Vector3 toPlayer = player.position - eyePosition; 
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > sightRange) return false; 


        
        if (Physics.Raycast(eyePosition, toPlayer.normalized, out RaycastHit hit, distanceToPlayer))
        {
            Transform hitTransform = hit.collider.transform;
            bool hitPlayer = hitTransform == player || hitTransform.IsChildOf(player);

            if (hitPlayer)
            {
                LastKnownPosition = player.position;
                return true;
            }
        }

        return false;
    }

    public bool AttackTarget()
    {
        Vector3 toPlayer = player.position - transform.position;

        toPlayer.y = 0;
        float distance = toPlayer.magnitude;

        if (distance > attackRange) return false;

        toPlayer.Normalize();
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        float dotProduct = Vector3.Dot(forward, toPlayer);
        return dotProduct >= attackCone;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        SetStateColor(Color.red);
        ChangeState(Stun);

        if (health <= 0)
        {
            SetStateColor(Color.red);
            GameObject _rm = GameObject.Find("RoomManager");
            RoomManager _rmScript = _rm.GetComponent<RoomManager>();
            _rmScript.KillEnemy(gameObject);
        }
    }


    public void SetStateColor(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }

    public void SetInversion()
    {
        if (isInversed)
            col.enabled = false;
        else
            col.enabled = true;
    }
}