using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField, Range(0,2)] private float _moveSpeed;
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _rotationOffset = 180;

    private Transform _target;

    private bool InStopDistance => (transform.position - _target.position).magnitude < _stopDistance;

    public void Initialize(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        if (_target == null || InStopDistance)
            return;

        Chase();
    }

    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _moveSpeed * Time.fixedDeltaTime);
        Rotate();
    }

    private void Rotate()
    {
        var distance = transform.position - _target.position;
        var rotation = Quaternion.LookRotation(distance);
        transform.rotation = Quaternion.AngleAxis(rotation.eulerAngles.y + _rotationOffset, Vector3.up);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _stopDistance);
    }
}