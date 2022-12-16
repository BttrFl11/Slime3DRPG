using TMPro;
using UnityEngine;

public class MoneyTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        PlayerStats.OnMoneyChanged += UpdateText;
    }

    private void OnDisable()
    {
        PlayerStats.OnMoneyChanged -= UpdateText;
    }

    private void UpdateText(float newValue)
    {
        _text.text = newValue.ToString("0");
    }
}