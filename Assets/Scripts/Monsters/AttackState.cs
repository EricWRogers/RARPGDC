using UnityEngine;

public class AttackState : MonsterIState
{
    private EnemyAI enemy;
    private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    public void OnEnter(EnemyAI context)
    {
        this.enemy = context;
        enemy.SetStateColor(Color.red);

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
            ExecuteAttack();
        }
    }

    private void ExecuteAttack()
    {
        Debug.Log("Mstrike");

        //enemy.GetComponent<Animator>().SetTrigger("Attack");

        nextAttackTime = 0f;
    }

    public void OnExit()
    {
        Debug.Log("exit attack");
    }



}
