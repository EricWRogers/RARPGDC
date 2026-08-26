using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shockwave : MonoBehaviour
{
    public float sinkDistance = 0.5f;
    public float sinkDuration = 0.5f;
    private readonly HashSet<EnemyAI> enemiesHit = new HashSet<EnemyAI>();

    void Start()
    {
        float duration = Mathf.Max(sinkDuration, 0.01f);
        transform.DOMoveY(transform.position.y - sinkDistance, duration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => Destroy(gameObject));
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyAI enemy = other.GetComponentInParent<EnemyAI>();
        if (enemy == null || !enemiesHit.Add(enemy)) return;

        Weapon weapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Weapon>();
        enemy.TakeDamage(weapon.damage);
        Debug.Log("Shockwave hit!");
    }
    
}
