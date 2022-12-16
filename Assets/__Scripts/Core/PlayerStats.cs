using System;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    [SerializeField] private PlayerStatsSO _playerStatsSO;
    [SerializeField] private float _startMoney;

    private float _money;
    public float Money
    {
        get => _money;
        set
        {
            _money = value;
            if (_money < 0)
                _money = 0;

            OnMoneyChanged?.Invoke(_money);
        }
    }

    public float Damage => _playerStatsSO.Damage;
    public float AttackRate => _playerStatsSO.AttackRate;
    public float Health => _playerStatsSO.MaxHealth;
    public float HealthRegeneration => _playerStatsSO.HealthRegeneration;

    public static event Action<float> OnMoneyChanged;
    public static event Action<float> OnAttackRateChanged;

    private void Start()
    {
        Money = _startMoney;
    }

    public void AddMoney(float amount)
    {
        Money += amount;
    }

    public void RemoveMoney(float amount)
    {
        Money -= amount;
    }

    public void IncreaseHealth(float amount)
    {
        _playerStatsSO.MaxHealth += amount;
    }

    public void IncreaseDamage(float amount)
    {
        _playerStatsSO.Damage += amount;
    }

    public void IncreaseHPRegeneration(float amount)
    {
        _playerStatsSO.HealthRegeneration += amount;
    }

    public void IncreaseAttackRate(float amount)
    {
        _playerStatsSO.AttackRate += amount;

        OnAttackRateChanged?.Invoke(_playerStatsSO.AttackRate);
    }
}