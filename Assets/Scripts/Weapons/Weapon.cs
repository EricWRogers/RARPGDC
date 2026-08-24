using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public GameObject sprite;
    public Animator animator;

    //public GameObject weaponType;

    public void Attack()
    {
        //play attack animation (dotween?)
        PerformAnimation();
        
        //detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //apply damage 
        foreach(Collider enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name + " for " + damage + " damage.");
        }
    }

    void PerformAnimation()
    {
        transform.DOLocalMove(new Vector3(.8f,0,.5f), 0.6f).SetEase(Ease.InOutBack);
        //transform.DOMove(new Vector3(0,0,0), 0.6f).SetEase(Ease.InOutBack).From();
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
