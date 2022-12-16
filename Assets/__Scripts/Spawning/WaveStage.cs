using UnityEngine;

[System.Serializable]
public struct WaveStage
{
    [SerializeField] private BaseEnemy[] _enemyVariants;
    [SerializeField] private int _enemyCount;

    public int EnemyCount => _enemyCount;
    public BaseEnemy[] EnemyVariants => _enemyVariants;
}
