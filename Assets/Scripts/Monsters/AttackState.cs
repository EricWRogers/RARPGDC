using System.Collections;
using UnityEngine;

public class AttackState : MonsterIState
{
    private EnemyAI enemy;
    private float attackCooldown = 2.5f;
    public float nextAttackTime = 0f;
    [Header("Damage")]
    public int damage;

    public SimplePlayerController playerScript;

    public void OnEnter(EnemyAI context)
    {
        // player = GameObject.FindWithTag("Player").transform;
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerScript = playerGO.GetComponent<SimplePlayerController>();

        this.enemy = context;
        // enemy.SetStateColor(Color.orange);

        nextAttackTime = attackCooldown;
        Debug.Log("enter atack");
    }

    public void OnUpdate()
    {
        if (!enemy.AttackTarget())
        {
            enemy.ChangeState(enemy.Chase);
            return;
        }
        
        if(nextAttackTime < attackCooldown)
        {
            nextAttackTime += Time.deltaTime;
        }

        if (nextAttackTime >= attackCooldown)
        {
            // ExecuteAttack();
            enemy.AttackFunction();
            nextAttackTime = 0f;
        }
    }

    // private void ExecuteAttack()
    // {
    //     //enemy.GetComponent<Animator>().SetTrigger("Attack");
        
        

        
    // }

    

    public void OnExit()
    {
        Debug.Log("exit attack");
    }
}
