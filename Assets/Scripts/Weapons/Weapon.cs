using UnityEngine;


public class Weapon : MonoBehaviour
{
    public int attackSpeed;
    public int chargeSpeed;
    public int rechageSpeed;
    public int damage;

    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    //public GameObject weaponType;

    public void Attack()
    {
        //play attack animation (dotween?)
        //detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //apply damage 
        foreach(Collider enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
        }
    }

    void OwGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
