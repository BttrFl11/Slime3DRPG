using System;
using UnityEngine;

public class Damageable<TStatsSO> : MonoBehaviour, IDamageable
    where TStatsSO : DamageableSO
{
    [SerializeField] protected TStatsSO _damageableSO;

    protected float _health;

    // arg1 - current health, arg2 - max health
    public event Action<float, float> OnHealthChanged;

    public TStatsSO DamageableSO => _damageableSO;
    public bool IsFullHP => (int)_health == (int)_damageableSO.MaxHealth; 

    protected virtual void Awake()
    {
        _health = _damageableSO.MaxHealth;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();

        OnHealthChanged?.Invoke(_health, _damageableSO.MaxHealth);
    }

    public void Heal(float amount)
    {
        _health += amount;
        if (_health > _damageableSO.MaxHealth)
            _health = _damageableSO.MaxHealth;

        OnHealthChanged?.Invoke(_health, _damageableSO.MaxHealth);
    }
}