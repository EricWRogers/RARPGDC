using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public float animationDelay;
    public List<float> recentHitTimers;
    public List<GameObject> recentHits;
    private Sequence _attackSequence;

    //public GameObject weaponType;

    public void Attack()
    {
        if (_attackSequence != null && _attackSequence.IsActive()) return;

        //play attack animation (dotween?)
        _attackSequence = PerformAnimation();
        
        //detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        HashSet<EnemyAI> enemiesHitThisAttack = new HashSet<EnemyAI>();

        //apply damage 
        foreach(Collider enemy in hitEnemies)
        {
            EnemyAI target = enemy.GetComponentInParent<EnemyAI>();
            if (target == null || !enemiesHitThisAttack.Add(target)) continue;

            target.TakeDamage(damage);
            Debug.Log("Hit " + target.name + " for " + damage + " damage.");
        }
    }

    Sequence PerformAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(new Vector3(.5f,0,.5f), animationDelay).SetEase(Ease.InQuint));
        sequence.Append(transform.DOLocalMove(new Vector3(0,0,0), animationDelay));
        sequence.Append(transform.DOLocalMove(new Vector3(-.5f,0,.5f), animationDelay).SetEase(Ease.InQuint));
        sequence.Append(transform.DOLocalMove(new Vector3(0,0,0), animationDelay).SetEase(Ease.InOutQuint));
        sequence.OnComplete(() => _attackSequence = null);
        return sequence;
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);


        
    }

    void Update()
    {
        for(int i = 0; i < recentHitTimers.Count; i++)
        {
            recentHitTimers[i] -= Time.deltaTime;

            if(recentHitTimers[i] <= 0)
            {
                recentHits.RemoveAt(i);
                recentHitTimers.RemoveAt(i--);
            }
        }


    }


}
