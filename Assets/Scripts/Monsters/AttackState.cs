using UnityEngine;

public class AttackState : MonsterIState
{
    private EnemyAI enemy;
    private float attackCooldown = 2.5f;
    private float nextAttackTime = 0f;
    [Header("Damage")]
    public float damage;

    private SimplePlayerController playerScript;

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
            ExecuteAttack();
        }
    }

    private void ExecuteAttack()
    {
        //enemy.GetComponent<Animator>().SetTrigger("Attack");
        damage = Random.Range(1f, 2f);

        enemy.SetStateColor(Color.pink);
        playerScript.TakeDamage(damage);

        nextAttackTime = 0f;
        Debug.Log("Monster strikes for " + damage + " damage.");
    }

    public void OnExit()
    {
        Debug.Log("exit attack");
    }
}
