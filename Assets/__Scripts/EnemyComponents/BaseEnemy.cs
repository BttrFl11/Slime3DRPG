using System;

public class BaseEnemy : Damageable<EnemyStatsSO>
{
    public static event Action OnEnemyDied;

    public UnitSO DamageableSO => _damageableSO;

    public override void Die()
    {
        OnEnemyDied?.Invoke();

        PlayerStats.Instance.AddMoney(_damageableSO.RewardForDeath);

        Destroy(gameObject);
    }
}
