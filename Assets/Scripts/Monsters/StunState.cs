
using UnityEngine;

public class StunState : MonsterIState
{
    private EnemyAI enemy;
    private float waitTimer;
    

    public void OnEnter(EnemyAI context)
    {
        enemy = context;
        waitTimer = 0f;
        enemy.SetStateColor(Color.yellow);
        enemy.Agent.isStopped = true;
        Debug.Log("has been stunned");
    }

    public void OnUpdate()
    {
        waitTimer += Time.deltaTime;

        if (waitTimer > enemy.stunTimer)
        {
            waitTimer = 0;
            enemy.ChangeState(enemy.Search);
        }
    }

    public void OnExit()
    {
        enemy.Agent.isStopped = false;
    }
}
