using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 0.5f)] private float _rotationSpeed;
    [SerializeField] private float _rotationOffset;

    private bool _isMoving;
    private PlayerAttacking _playerAttacking;

    public event Action OnPlayerMove;
    public event Action OnPlayerStop;

    public bool IsMoving => _isMoving;

    private void Awake()
    {
        _playerAttacking = GetComponent<PlayerAttacking>();
    }

    public void OnEnable()
    {
        EnemySpawner.OnStageEnded += StartMove;
        EnemySpawner.OnStageStart += StopMove;
        GameStateManager.OnGameEnded += StopMove;
    }

    public void OnDisable()
    {
        EnemySpawner.OnStageEnded -= StartMove;
        EnemySpawner.OnStageStart -= StopMove;
        GameStateManager.OnGameEnded -= StopMove;
    }

    private void FixedUpdate()
    {
        if (_playerAttacking.Target != null)
            Rotate();
    }

    private void Rotate()
    {
        var target = _playerAttacking.Target.transform;
        var distance = target.position - transform.position;
        var lookRotation = Quaternion.LookRotation(distance);
        var rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSpeed).eulerAngles;
        transform.rotation = Quaternion.AngleAxis(rotation.y + _rotationOffset, Vector3.up);
    }

    private void StartMove()
    {
        // play move animation

        _isMoving = true;

        OnPlayerMove?.Invoke();
    }

    private void StopMove()
    {
        // play idle animation

        _isMoving = false;

        OnPlayerStop?.Invoke();
    }
}