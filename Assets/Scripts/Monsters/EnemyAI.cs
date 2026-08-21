using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection settings")]
    public Transform player;
    public float sightRange = 8f;      // How far it can notice you
    public LayerMask obstacleMask;     // Layer for walls and other occluders

    [Header("Movement settings")]
    public float moveSpeed = 2f;       // Patrol / search speed
    public float chaseSpeed = 4f;      // Chase speed

    // "Last seen position" used by chase and search
    public Vector3 LastKnownPosition { get; set; }

    // Create one set of states up front and reuse them (no new each time)
    public PatrolState Patrol { get; private set; }
    public ChaseState Chase { get; private set; }
    public SearchState Search { get; private set; }

    private MonsterIState currentState;

    void Awake()
    {
        Patrol = new PatrolState();
        Chase = new ChaseState();
        Search = new SearchState();
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

        // Blocked by a wall means not visible
        if (Physics.Raycast(transform.position, toPlayer.normalized,
                toPlayer.magnitude, obstacleMask)) return false;

        LastKnownPosition = player.position; // Keep updating while visible
        return true;
    }

    // Helper to make the current state visible as a color (call from each state's OnEnter)
    public void SetStateColor(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }
}