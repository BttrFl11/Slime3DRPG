using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyWaveSO[] _waves;
    [SerializeField] private float _timeBtwWaves;
    [SerializeField] private float _timeBtwStages;
    [SerializeField] private Vector2 _enemySpawnStartPosXZ, _enemySpawnEndPosXZ;

    private EnemyWaveSO _currentWave;
    private int _enemiesAlive;

    public int EnemiesAlive => _enemiesAlive;

    public static event Action OnStageEnded;
    public static event Action OnStageStart;
    public static event Action OnAllWavesEnded;

    private void OnEnable()
    {
        GameStateManager.OnGameStarted += OnGameStarted;
        BaseEnemy.OnEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameStarted -= OnGameStarted;
        BaseEnemy.OnEnemyDied -= OnEnemyDied;
    }

    private void OnGameStarted()
    {
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        OnStageEnded?.Invoke();
        yield return new WaitForSeconds(_timeBtwStages);

        for (int i = 0; i < _waves.Length; i++)
        {
            yield return new WaitForSeconds(_timeBtwWaves);

            _currentWave = _waves[i];

            for (int j = 0; j < _currentWave.WaveStages.Length; j++)
            {
                yield return new WaitForSeconds(_timeBtwStages);

                print("starting new wave");
                OnStageStart?.Invoke();

                var stage = _currentWave.WaveStages[j];
                for (int l = 0; l < stage.EnemyCount; l++)
                {
                    var pos = GetRandomSpawnPosition();
                    var enemy = GetRangomEnemy(stage.EnemyVariants);
                    SpawnSingleEnemy(enemy, pos);
                }

                yield return new WaitUntil(() => EnemiesAlive == 0);

                OnStageEnded?.Invoke();
            }
        }

        yield return new WaitUntil(() => EnemiesAlive == 0);

        OnAllWavesEnded?.Invoke();

        yield return null;
    }

    private void OnEnemyDied()
    {
        _enemiesAlive--;
    }

    private BaseEnemy GetRangomEnemy(BaseEnemy[] enemyVariants)
    {
        var randEnemy = enemyVariants[Range(0, enemyVariants.Length)];
        return randEnemy;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var x = Range(_enemySpawnStartPosXZ.x, _enemySpawnEndPosXZ.x);
        var z = Range(_enemySpawnStartPosXZ.y, _enemySpawnEndPosXZ.y);
        return new Vector3(x, 0, z);
    }

    private void SpawnSingleEnemy(BaseEnemy enemy, Vector3 pos)
    {
        var spawnedEnemy = Instantiate(enemy, pos, Quaternion.Euler(0,-90,0));
        _enemiesAlive++;
    }
}