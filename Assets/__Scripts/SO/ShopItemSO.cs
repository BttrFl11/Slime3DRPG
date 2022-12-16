using UnityEngine;

[CreateAssetMenu(menuName = "SO/ShopItem")]
public class ShopItemSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _valueFormat = "0";
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private Sprite _icon;

    public float Price;
    public int Level = 1;
    public float IncreaseValuePerLvl;

    public string Name => _name;
    public string ValueFormat => _valueFormat;
    public UpgradeType UpgradeType => _upgradeType;
    public Sprite Icon => _icon;
}