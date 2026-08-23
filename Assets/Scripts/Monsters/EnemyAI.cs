using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection settings")]
    public Transform player { get; private set; }
    public float sightRange = 8f;      // How far it can notice you
    public LayerMask obstacleMask;     // Layer for walls and other occluders

    [Header("Attack settings")]
    public float attackRange = 2.0f;
    [Range(-1f,1f)]
    public float attackCone = 0.5f;

    [Header("Movement settings")]
    public float moveSpeed = 2f;      
    public float chaseSpeed = 4f;     

    // "Last seen position" used by chase and search
    public Vector3 LastKnownPosition { get; set; }

    // Create one set of states up front and reuse them (no new each time)
    public PatrolState Patrol { get; private set; }
    public ChaseState Chase { get; private set; }
    public SearchState Search { get; private set; }
    public AttackState Attack { get; private set; }

    private MonsterIState currentState;

    void Awake()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
        }
        else
        {
            Debug.LogError($"WHERE IS THE PLAYER??? {gameObject.name} CANT FIND ANYTHING TAGGED PLAYER!");
        }

        Patrol = new PatrolState();
        Chase = new ChaseState();
        Search = new SearchState();
        Attack = new AttackState();
    }

    void Start() { ChangeState(Patrol); }

    void Update() { currentState?.OnUpdate(); }

    public void ChangeState(MonsterIState nextState)
    {
        currentState?.OnExit();
        currentState = nextState;
        currentState.OnEnter(this);
    }

    // The real vision check: visible if "within range" AND "no wall in between"
    public bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude > sightRange) return false;

        
        if (Physics.Raycast(transform.position, toPlayer.normalized,
                toPlayer.magnitude, obstacleMask)) return false;

        LastKnownPosition = player.position; 
        return true;
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


    public void SetStateColor(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }
}