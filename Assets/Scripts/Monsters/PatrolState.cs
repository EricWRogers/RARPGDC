using UnityEngine;

public class PatrolState : MonsterIState
{
    public EnemyAI enemy;
    public void OnEnter(EnemyAI context)
    {
        this.enemy = context;
        Debug.Log("Entering Patrol State");
    }

    public void OnUpdate()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState());
        }
    }

    public void OnExit()
    {
        
    }
}
