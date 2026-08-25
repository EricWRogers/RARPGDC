using UnityEngine;

public class AttackState : MonsterIState
{
    private EnemyAI enemy;
    private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;
    [Header("Damage")]
    public int damage;

    private SimplePlayerController playerScript;

    public void OnEnter(EnemyAI context)
    {
        // player = GameObject.FindWithTag("Player").transform;
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerScript = playerGO.GetComponent<SimplePlayerController>();

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
        //enemy.GetComponent<Animator>().SetTrigger("Attack");
        damage = Random.Range(1, 3);

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
