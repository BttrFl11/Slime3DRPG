using UnityEngine;
using UnityEngine.UI;

public class HealthBarGUI : MonoBehaviour
{
    [SerializeField] private Image _healthImage;

    private Camera _camera;
    private IDamageable _damageable;

    private void Awake()
    {
        _damageable = GetComponentInParent<IDamageable>();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _damageable.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        _damageable.OnHealthChanged -= UpdateHealth;
    }

    private void LateUpdate()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        transform.LookAt(transform.position + _camera.transform.forward);
    }

    private void UpdateHealth(float health, float maxHealth)
    {
        _healthImage.fillAmount = health / maxHealth;
    }
}
