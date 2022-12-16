using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyStats")]
public class EnemyStatsSO : UnitSO
{
    [SerializeField] private float _rewardForDeath;

    public float RewardForDeath => _rewardForDeath;
}