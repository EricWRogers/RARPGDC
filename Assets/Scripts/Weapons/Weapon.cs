using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Spear,
        Fireball,
        Mace
    }

    public WeaponType myType;
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
    public Light weaponLight;
    public float fireballLightIntensity = 2f;
    public float fireballForwardDistance = 0.15f;
    public float fireballRecoilDistance = 0.1f;
    private Vector3 _restingBodyLocalPosition;
    private Vector3 _restingBodyLocalEulerAngles;
    private float _restingLightIntensity;
    public GameObject fireBallPrefab;
    private SimplePlayerController player;
    public float maceRestingHeight = 0.15f;
    public float maceRestingAngle = -55f;
    public float maceSlamHeight = -0.1f;

    //public GameObject weaponType;

    void Awake()
    {
        player = FindFirstObjectByType<SimplePlayerController>();

        _restingLocalPosition = transform.localPosition;
        if (weaponBody != null)
        {
            _restingBodyLocalPosition = weaponBody.transform.localPosition;
            _restingBodyLocalEulerAngles = weaponBody.transform.localEulerAngles;
            if (myType == WeaponType.Mace)
            {
                weaponBody.transform.localPosition = _restingBodyLocalPosition + Vector3.up * maceRestingHeight;
                weaponBody.transform.localRotation = Quaternion.Euler(_restingBodyLocalEulerAngles + new Vector3(maceRestingAngle, 0f, 0f));
            }
        }
        if (weaponLight != null)
            _restingLightIntensity = weaponLight.intensity;
    }

    public void Attack()
    {
        if (_attackSequence != null && _attackSequence.IsActive()) return;

        //play attack animation (dotween!)
        if(myType == WeaponType.Spear)
            _attackSequence = PerformSpearAnimation();

        else if(myType == WeaponType.Fireball)
            _attackSequence = PerformFireballAnimation();

        else if(myType == WeaponType.Mace)
            _attackSequence = PerformMaceAnimation();
    }

    Sequence PerformFireballAnimation()
    {
        if (weaponBody == null || weaponLight == null) return DOTween.Sequence();

        float fadeDuration = Mathf.Max(animationStepLength, 0.01f);
        Vector3 localForward = weaponBody.transform.localRotation * Vector3.forward;
        Vector3 forwardPosition = _restingBodyLocalPosition + localForward * fireballForwardDistance;
        Vector3 recoilPosition = _restingBodyLocalPosition - localForward * fireballRecoilDistance;

        weaponLight.intensity = 0f;
        weaponLight.enabled = true;

        Sequence sequence = DOTween.Sequence();
        //Wind up
        sequence.AppendCallback(() => player.aimingDisabled = true);
        sequence.Append(weaponLight.DOIntensity(fireballLightIntensity, fadeDuration).SetEase(Ease.InOutQuad));
        sequence.Join(weaponBody.transform.DOLocalMove(forwardPosition, fadeDuration).SetEase(Ease.InOutQuad));
        //Shoot fireball
        sequence.AppendCallback(() => Instantiate(fireBallPrefab, attackPoint.position, transform.rotation));
        //Recoil
        sequence.Append(weaponBody.transform.DOLocalMove(recoilPosition, fadeDuration * 0.25f).SetEase(Ease.OutQuad));
        //Return
        sequence.Append(weaponBody.transform.DOLocalMove(_restingBodyLocalPosition, fadeDuration * 0.75f).SetEase(Ease.OutQuad));
        sequence.Join(weaponLight.DOIntensity(_restingLightIntensity, fadeDuration * 0.5f));
        sequence.AppendCallback(() => weaponLight.enabled = _restingLightIntensity > 0f);
        sequence.AppendCallback(() => player.aimingDisabled = false);
        sequence.OnComplete(() => _attackSequence = null);
        sequence.OnKill(() =>
        {
            weaponBody.transform.localPosition = _restingBodyLocalPosition;
            weaponLight.intensity = _restingLightIntensity;
            weaponLight.enabled = _restingLightIntensity > 0f;
            player.aimingDisabled = false;
            _attackSequence = null;
        });
        return sequence;
    }

    Sequence PerformSpearAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        Vector3 localForward = transform.localRotation * -Vector3.forward;
        Vector3 jabPosition = _restingLocalPosition + localForward * spearJabDistance;
        float stepDuration = Mathf.Max(animationStepLength, 0.01f);

        sequence.AppendCallback(() => player.aimingDisabled = true);
        sequence.Append(
            transform.DOLocalMove(jabPosition, stepDuration)
                .SetEase(Ease.OutQuad)
        );

        sequence.AppendCallback(ApplyDamage);

        sequence.Append(
            transform.DOLocalMove(_restingLocalPosition, stepDuration * 1.5f)
                .SetEase(Ease.InQuad)
        );

        sequence.AppendCallback(() => player.aimingDisabled = false);
        sequence.OnComplete(() => _attackSequence = null);
        sequence.OnKill(() => 
        {
            _attackSequence = null;
            player.aimingDisabled = false;
        
        });

        return sequence;
    }

    Sequence PerformMaceAnimation()
    {
        if (weaponBody == null) return DOTween.Sequence();

        float stepDuration = Mathf.Max(animationStepLength, 0.01f);
        Vector3 restingPosition = _restingBodyLocalPosition + Vector3.up * maceRestingHeight;
        Vector3 windupPosition = restingPosition + Vector3.up * maceRestingHeight;
        Vector3 slamPosition = _restingBodyLocalPosition + Vector3.up * maceSlamHeight;
        Vector3 restingRotation = _restingBodyLocalEulerAngles + new Vector3(maceRestingAngle, 0f, 0f);
        Vector3 windupRotation = restingRotation + new Vector3(-25f, 0f, 0f);
        Vector3 slamRotation = restingRotation + new Vector3(130f, 0f, 0f);
        Transform maceTransform = weaponBody.transform;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => player.aimingDisabled = true);
        sequence.Append(maceTransform.DOLocalMove(windupPosition, stepDuration).SetEase(Ease.OutQuad));
        sequence.Join(maceTransform.DOLocalRotate(windupRotation, stepDuration).SetEase(Ease.OutQuad));
        sequence.Append(maceTransform.DOLocalMove(slamPosition, stepDuration * 0.35f).SetEase(Ease.InQuad));
        sequence.Join(maceTransform.DOLocalRotate(slamRotation, stepDuration * 0.35f).SetEase(Ease.InQuad));
        sequence.AppendCallback(ApplyDamage);
        sequence.Append(maceTransform.DOLocalMove(restingPosition, stepDuration * 0.75f).SetEase(Ease.OutQuad));
        sequence.Join(maceTransform.DOLocalRotate(restingRotation, stepDuration * 0.75f).SetEase(Ease.OutQuad));
        sequence.AppendCallback(() => player.aimingDisabled = false);
        sequence.OnComplete(() => _attackSequence = null);
        sequence.OnKill(() =>
        {
            maceTransform.localPosition = restingPosition;
            maceTransform.localRotation = Quaternion.Euler(restingRotation);
            player.aimingDisabled = false;
            _attackSequence = null;
        });
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
