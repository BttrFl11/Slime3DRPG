using System;

public interface IDamageable
{
    void TakeDamage(float damage);
    void Die();
    event Action<float, float> OnHealthChanged;
}