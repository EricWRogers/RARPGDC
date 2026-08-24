using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public float animationDelay;

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
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(new Vector3(.5f,0,.5f), animationDelay).SetEase(Ease.InQuint));
        sequence.Append(transform.DOLocalMove(new Vector3(0,0,0), animationDelay));
        sequence.Append(transform.DOLocalMove(new Vector3(-.5f,0,.5f), animationDelay).SetEase(Ease.InQuint));
        sequence.Append(transform.DOLocalMove(new Vector3(0,0,0), animationDelay).SetEase(Ease.InOutQuint));
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
