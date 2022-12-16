using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopItemPanel : MonoBehaviour
{
    [SerializeField] private ShopItemSO _shopItemSO;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [SerializeField] private TextMeshProUGUI _valueAmount;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _upgradeButton;

    private void Awake()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        PlayerStats.OnMoneyChanged += UpdateButtonInteraction;
    }

    private void OnDisable()
    {
        PlayerStats.OnMoneyChanged -= UpdateButtonInteraction;
    }

    private void UpdateUI()
    {
        _levelText.text = $"Lv {_shopItemSO.Level}";
        _itemIcon.sprite = _shopItemSO.Icon;
        _itemName.text = _shopItemSO.Name;
        _itemPrice.text = _shopItemSO.Price.ToString("0");
        float valueAmount = GetValueByType(_shopItemSO.UpgradeType);
        _valueAmount.text = valueAmount.ToString(_shopItemSO.ValueFormat);
    }

    private float GetValueByType(UpgradeType type)
    {
        var player = PlayerStats.Instance;
        switch (type)
        {
            case UpgradeType.Damage:
                return player.Damage;
            case UpgradeType.MaxHealth:
                return player.Health;
            case UpgradeType.HealthRegeneration:
                return player.HealthRegeneration;
            case UpgradeType.AttackRate:
                return player.AttackRate;
        }

        return 0;
    }

    private void UpdateButtonInteraction(float money)
    {
        _upgradeButton.interactable = money >= _shopItemSO.Price;
    }

    public void OnUpgradeButton()
    {
        var player = PlayerStats.Instance;
        if (player.Money < _shopItemSO.Price)
            throw new InvalidOperationException("Do not have enough money");

        player.RemoveMoney(_shopItemSO.Price);

        var amount = _shopItemSO.IncreaseValuePerLvl;
        switch (_shopItemSO.UpgradeType)
        {
            case UpgradeType.Damage:
                player.IncreaseDamage(amount);
                break;
            case UpgradeType.MaxHealth:
                player.IncreaseHealth(amount);
                break;
            case UpgradeType.HealthRegeneration:
                player.IncreaseHPRegeneration(amount);
                break;
            case UpgradeType.AttackRate:
                player.IncreaseAttackRate(amount);
                break;
        }

        _shopItemSO.Level++;

        UpdateUI();
    }
}
