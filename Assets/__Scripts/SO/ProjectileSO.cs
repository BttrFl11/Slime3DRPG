using UnityEngine;

[CreateAssetMenu(menuName = "SO/Projectile")]
public class ProjectileSO : ScriptableObject
{
    [SerializeField] private AnimationCurve _trajectory;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxYAtDistance;

    public GameObject ProjectilePrefab => _projectilePrefab;
    public float Speed => _speed;
    public AnimationCurve Trajectory => _trajectory;
    public float MaxYAtDistance => _maxYAtDistance;
}
