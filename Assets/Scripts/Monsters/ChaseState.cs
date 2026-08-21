using UnityEngine;

public class ChaseState : MonsterIState
{
    private EnemyAI enemy;
    private float lostTimer; // Time since losing sight

    public void OnEnter(EnemyAI context)
    {
        enemy = context;
        lostTimer = 0f; // We reuse instances, so always reset on entry
        enemy.SetStateColor(Color.red);
    }

    public void OnUpdate()
    {
        if (enemy.CanSeePlayer())
        {
            lostTimer = 0f; // Reset the grace while the player is visible
        }
        else
        {
            // Don't give up the instant you lose sight. Hang on for 2 seconds
            lostTimer += Time.deltaTime;
            if (lostTimer >= 2f)
            {
                enemy.ChangeState(enemy.Search);
                return;
            }
        }

        // Head for the last seen position (keep running during the grace period too)
        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position, enemy.LastKnownPosition,
            enemy.chaseSpeed * Time.deltaTime);
    }

    public void OnExit() { }
}