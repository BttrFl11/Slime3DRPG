using System.Collections;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour, IAttackable
{
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _targetLayer;

    private Animator _animator;
    private Enemy _enemy;
    private float _startTimeBtwAttacks;
    private float _timeBtwAttacks;
    private bool _isAttacking;

    private bool CanAttack => _isAttacking == false && _timeBtwAttacks <= 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();

        _startTimeBtwAttacks = 1 / _enemy.DamageableSO.AttackRate;
        _timeBtwAttacks = _startTimeBtwAttacks;
    }

    private void FixedUpdate()
    {
        if (_enemy.Target == null)
            return;

        _timeBtwAttacks -= Time.fixedDeltaTime;

        if (CanAttack)
            CheckTarget();
    }

    private void CheckTarget()
    {
        if ((_enemy.Target.position - transform.position).magnitude < _attackRadius)
            Attack();
    }

    private void PlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");

        _isAttacking = true;
    }

    private void ApplyDamage(IDamageable damageable)
    {
        damageable.TakeDamage(_enemy.DamageableSO.Damage);
    }

    // should invoke in animation
    public void AnimatorAttack()
    {
        if (_enemy.Target.TryGetComponent(out IDamageable damageable))
            ApplyDamage(damageable);

        _timeBtwAttacks = _startTimeBtwAttacks;
        _isAttacking = false;
    }

    public void Attack()
    {
        PlayAttackAnimation();
    }
}