using System;
using System.Collections;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour, IAttackable
{
    [SerializeField] private ProjectileSO _projectileSO;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRadius;

    private Player _player;
    private float _startTimeBtwAttacks;
    private float _timeBtwAttacks;
    private BaseEnemy _target;

    private readonly Collider[] _enemiesInRange = new Collider[1];

    public Action<BaseEnemy> OnTargetChanged;

    public BaseEnemy Target => _target;
    public bool CanAttack => _timeBtwAttacks <= 0
        && GameStateManager.Instance.CurrentState == GameState.Playing
        && EnemiesInRange() > 0;

    private void Awake()
    {
        _player = GetComponent<Player>();

        _startTimeBtwAttacks = 1 / _player.PlayerStatsSO.AttackRate;
        _timeBtwAttacks = _startTimeBtwAttacks;
    }

    private void OnEnable()
    {
        EnemySpawner.OnStageStart += ResetTimeBtwAttacks;
        PlayerStats.OnAttackRateChanged += UpdateAttackRate;
    }

    private void OnDisable()
    {
        EnemySpawner.OnStageStart -= ResetTimeBtwAttacks;
        PlayerStats.OnAttackRateChanged -= UpdateAttackRate;
    }

    private void FixedUpdate()
    {
        _timeBtwAttacks -= Time.fixedDeltaTime;
    }

    private int EnemiesInRange()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, _attackRadius, _enemiesInRange, _enemyLayer);
    }

    private void ResetTimeBtwAttacks()
    {
        _timeBtwAttacks = _startTimeBtwAttacks;
    }

    private BaseEnemy GetNearestEnemy()
    {
        var enemies = FindObjectsOfType<BaseEnemy>();
        var nearestEnemy = enemies[0];
        var nearestDistance = (transform.position - nearestEnemy.transform.position).magnitude;
        for (int i = 1; i < enemies.Length; i++)
        {
            if ((transform.position - enemies[i].transform.position).magnitude < nearestDistance)
                nearestEnemy = enemies[i];
        }
        return nearestEnemy;
    }

    private IEnumerator SimulateProjectile()
    {
        var enemy = _target = GetNearestEnemy();
        OnTargetChanged?.Invoke(_target);
        if (_target != null)
        {
            var projectile = Instantiate(_projectileSO.ProjectilePrefab, transform.position, Quaternion.identity);
            var startDistance = (transform.position - enemy.transform.position).magnitude;
            var time = startDistance / _projectileSO.Speed;
            var startTime = time;
            var direction = (enemy.transform.position - transform.position).normalized;
            var trajectory = _projectileSO.Trajectory;
            var lastKey = trajectory.keys[trajectory.length - 1];
            var cachedDistance = startDistance;

            while (time > 0)
            {
                // updating distance to the target
                float distance = cachedDistance;
                if (enemy != null)
                     distance = (transform.position - enemy.transform.position).magnitude;
                var distanceDelta = cachedDistance - distance;
                cachedDistance = distance;

                // moving the projectile X position
                if (_target != null)
                    direction = (_target.transform.position - transform.position).normalized;
                projectile.transform.Translate(_projectileSO.Speed * Time.fixedDeltaTime * direction);

                // calculating the projectile Y position by the animation curve
                var keyTime = 1 - ((time / startTime) / lastKey.time);
                Vector3 newPos = projectile.transform.position;
                float posY = Mathf.Clamp01(distance / _projectileSO.MaxYAtDistance) * trajectory.Evaluate(keyTime);
                newPos.y = posY;
                projectile.transform.position = newPos;

                // decreases the time by realtime + changed distance btw startPos and target
                time -= Time.fixedDeltaTime + distanceDelta / _projectileSO.Speed;
                yield return new WaitForFixedUpdate();
            }

            // applying the damage
            if (_target != null)
                _target.TakeDamage(_player.PlayerStatsSO.Damage);
            Destroy(projectile);
        }
    }

    private void UpdateAttackRate(float newRate)
    {
        _startTimeBtwAttacks = 1 / newRate;
    }

    public void Attack()
    {
        StartCoroutine(SimulateProjectile());

        ResetTimeBtwAttacks();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}