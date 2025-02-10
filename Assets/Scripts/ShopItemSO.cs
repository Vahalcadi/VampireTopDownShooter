using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Hp,
    Armor,
    None
}



[CreateAssetMenu]
public class ShopItemSO : ScriptableObject
{
    public bool shouldAddStat;
    public bool shouldAddAmmo;
    public bool shouldAddWeapon;

    [SerializeField] StatType statToAdd;
    [SerializeField] WeaponType ammoToAdd;
    [SerializeField] private string itemName;
    [SerializeField] private float itemCost;
    [SerializeField] private Sprite icon;

    public WeaponType AmmoToAdd { get => ammoToAdd; }
    public StatType StatToAdd { get => statToAdd; }
    public string ItemName { get => itemName; }
    public float ItemCost { get => itemCost; set => itemCost = value; }
    public Sprite Icon { get => icon; }
}
