using UnityEngine;

public class GroundMover : MonoBehaviour
{
    [SerializeField] private float _groundEndPosX;
    [SerializeField] private float _movePosX;
    [SerializeField] private Transform[] _grounds;
    [SerializeField] private float _speed;

    private bool _move;
    private PlayerMovement _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>();
    }

    private void OnEnable()
    {
        _player.OnPlayerMove += () => _move = true;
        _player.OnPlayerStop += () => _move = false;
    }

    private void OnDisable()
    {
        _player.OnPlayerMove -= () => _move = true;
        _player.OnPlayerStop -= () => _move = false;
    }

    private void FixedUpdate()
    {
        if (_move)
            MoveGrounds();
    }

    private void MoveGrounds()
    {
        foreach (var ground in _grounds)
        {
            ground.Translate(_speed * Time.fixedDeltaTime * Vector2.left);
            if (ground.position.x < _groundEndPosX)
                ground.position = new Vector3(ground.position.x + _movePosX, ground.position.y, ground.position.z);
        }
    }
}
