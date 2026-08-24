using UnityEngine;

public class PatrolState : MonsterIState 
{ 
    private EnemyAI enemy; 
    private int currentWaypointIndex = 0;

    public void OnEnter(EnemyAI context) 
    { 
        this.enemy = context; 
        Debug.Log("Entering Patrol State"); 

        MoveToCurrentWaypoint();
    } 

    public void OnUpdate() 
    { 
        if (enemy.CanSeePlayer()) 
        { 
            enemy.ChangeState(enemy.Chase); 
            return;
        } 

        if (HasReachedDestination())
        {
            CycleToNextWaypoint();
        }
    } 

    public void OnExit() 
    { 
        if (enemy.Agent != null && enemy.Agent.isActiveAndEnabled)
        {
            enemy.Agent.ResetPath();
        }
    } 

    private void MoveToCurrentWaypoint()
    {
        if (enemy.waypoints == null || enemy.waypoints.Length == 0) return;

        Transform targetWaypoint = enemy.waypoints[currentWaypointIndex];
        if (targetWaypoint != null && enemy.Agent != null)
        {
            enemy.Agent.SetDestination(targetWaypoint.position);
        }
    }

    private void CycleToNextWaypoint()
    {
        if (enemy.waypoints == null || enemy.waypoints.Length == 0) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % enemy.waypoints.Length;
        MoveToCurrentWaypoint();
    }

    private bool HasReachedDestination()
    {
        if (enemy.Agent == null) return false;

        if (enemy.Agent.pathPending) return false;

        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            if (!enemy.Agent.hasPath || enemy.Agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }
}

