using UnityEngine;

public class SearchState : MonsterIState
{
    private EnemyAI enemy;
    private float searchTimer;

    public void OnEnter(EnemyAI context)
    {
        enemy = context;
        searchTimer = 0f;
        enemy.SetStateColor(Color.yellow);
        Debug.Log("enter search"); 
    }


    public void OnUpdate()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(enemy.Chase);
            // return;
        }

        enemy.transform.position = Vector3.MoveTowards(
        enemy.transform.position, enemy.LastKnownPosition,
        enemy.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(enemy.transform.position, enemy.LastKnownPosition) < 0.1f)
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= 3f) enemy.ChangeState(enemy.Wait);
        }
    }

    public void OnExit() { }
}