using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public float animationStepLength;
    public List<float> recentHitTimers;
    public List<GameObject> recentHits;
    private Sequence _attackSequence;
    private Vector3 _restingLocalPosition;
    public float spearJabDistance = 0.75f;
    public GameObject weaponBody;

    //public GameObject weaponType;

    void Awake()
    {
        _restingLocalPosition = transform.localPosition;
    }

    public void Attack()
    {
        if (_attackSequence != null && _attackSequence.IsActive()) return;

        //play attack animation (dotween!)
        _attackSequence = PerformSpearAnimation();
    }

    Sequence PerformSpearAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        Vector3 jabPosition = _restingLocalPosition + weaponBody.transform.forward * spearJabDistance;
        float stepDuration = Mathf.Max(animationStepLength, 0.01f);

        sequence.Append(
            transform.DOLocalMove(jabPosition, stepDuration)
                .SetEase(Ease.OutQuad)
        );

        sequence.AppendCallback(ApplyDamage);

        sequence.Append(
            transform.DOLocalMove(_restingLocalPosition, stepDuration)
                .SetEase(Ease.InQuad)
        );

        sequence.OnComplete(() => _attackSequence = null);
        sequence.OnKill(() => _attackSequence = null);
        return sequence;
    }

    void ApplyDamage()
    {
        if (attackPoint == null) return;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        HashSet<EnemyAI> enemiesHitThisAttack = new HashSet<EnemyAI>();

        foreach (Collider enemy in hitEnemies)
        {
            EnemyAI target = enemy.GetComponentInParent<EnemyAI>();
            if (target == null || !enemiesHitThisAttack.Add(target)) continue;

            target.TakeDamage(damage);
            Debug.Log("Hit " + target.name + " for " + damage + " damage.");
        }
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
