using UnityEngine;

public class ChaseState : MonsterIState
{
    private EnemyAI enemy;
    private float lostTimer; 

    public void OnEnter(EnemyAI context)
    {
        this.enemy = context;
        lostTimer = 0f; 
        enemy.SetStateColor(Color.red);
        Debug.Log("Chase state");
    }

    public void OnUpdate()
    {
        if (enemy.AttackTarget())
        {
            enemy.ChangeState(enemy.Attack);
        }



        if (enemy.CanSeePlayer())
        {
            lostTimer = 0f; 
            enemy.Agent.SetDestination(enemy.player.position);
        }
        else
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= 2f)
            {
                enemy.ChangeState(enemy.Search);
                return;
            }
        }

        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position, enemy.LastKnownPosition,
            enemy.chaseSpeed * Time.deltaTime);
    }

    public void OnExit() { }
}