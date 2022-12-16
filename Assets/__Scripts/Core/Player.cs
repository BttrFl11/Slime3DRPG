using UnityEngine;
using System;

[RequireComponent(typeof(PlayerAttacking), typeof(PlayerMovement))]
public class Player : Damageable<PlayerStatsSO>
{
    private PlayerAttacking _playerAttacking;
    private PlayerMovement _playerMovement;

    public PlayerStatsSO PlayerStatsSO => _damageableSO;

    public static event Action OnPlayerDied;

    protected override void Awake()
    {
        base.Awake();

        _playerAttacking = GetComponent<PlayerAttacking>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (_playerAttacking.CanAttack && _playerMovement.IsMoving == false)
            Attack();
    }

    private void FixedUpdate()
    {
        if(IsFullHP == false)
        {
            var healAmount = Time.fixedDeltaTime * _damageableSO.HealthRegeneration;
            Heal(healAmount);
        }
    }

    public override void Die()
    {
        OnPlayerDied?.Invoke();

        base.Die();
    }

    private void Attack()
    {
        _playerAttacking.Attack();
    }
}
