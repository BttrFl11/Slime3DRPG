using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyAttacking))]
public class Enemy : BaseEnemy
{
    protected EnemyMovement _enemyMovement;

    protected Transform _target;

    public Transform Target => _target;

    protected override void Awake()
    {
        base.Awake();

        _enemyMovement = GetComponent<EnemyMovement>();
        _target = FindObjectOfType<Player>().transform;

        if (_target != null)
            _enemyMovement.Initialize(_target.transform);
    }
}