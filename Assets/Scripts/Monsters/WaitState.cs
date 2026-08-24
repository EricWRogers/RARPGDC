using UnityEngine;

public class WaitState : MonsterIState
{
    private EnemyAI enemy; 

    public void OnEnter(EnemyAI context) 
    { 
        this.enemy = context; 
        Debug.Log("Entering wait State"); 

        // MoveToCurrentWaypoint();
    } 

    public void OnUpdate() 
    { 
        if (enemy.CanSeePlayer()) 
        { 
            // enemy.Agent.SetDestination(enemy.player.position);
            enemy.ChangeState(enemy.Chase);
        }
        else
        {
            // enemy.ChangeState(enemy.Search);
        }

        // if (HasReachedDestination())
        // {
        //     CycleToNextWaypoint();
        // }
    } 

    public void OnExit() 
    { 
        if (enemy.Agent != null && enemy.Agent.isActiveAndEnabled)
        {
            enemy.Agent.ResetPath();
        }
    } 
}
