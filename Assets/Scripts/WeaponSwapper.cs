using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponSwapper : MonoBehaviour
{
    public static Action<float> OnWeaponSwap;
    public static Func<List<Weapon>> UnlockedWeapons;

    [SerializeField] private Weapon[] allWeapons;
    public List<Weapon> unlockedWeapons;
    //private Weapon equippedWeapon;
    private int currentWeaponIndex;

    private void Awake()
    {
        currentWeaponIndex = 0;
        unlockedWeapons = allWeapons.ToList().FindAll(w => w.unlocked == true);
    }

    private void Start()
    {
        //equippedWeapon = weapons[0];
        foreach (Weapon weapon in allWeapons)
        {
            EventManager.OnAmmoConsumed?.Invoke(weapon.WeaponType, weapon.ammoInfos.CurrentMagazineAmmo.ToString(), weapon.ammoInfos.CurrentAmmoReserves.ToString());
        }
    }

    public void ChangeWeapon(float scrollValue)
    {
        if (GameManager.Instance.PreviousWeapon != null)
            return;

        unlockedWeapons[currentWeaponIndex].gameObject.SetActive(false);

        currentWeaponIndex += (int)-scrollValue;
        if (currentWeaponIndex < 0)
            currentWeaponIndex = unlockedWeapons.Count - 1;
        else if (currentWeaponIndex > unlockedWeapons.Count - 1)
            currentWeaponIndex = 0;

        unlockedWeapons[currentWeaponIndex].gameObject.SetActive(true);
        Player.OnMeshChanged?.Invoke(unlockedWeapons[currentWeaponIndex].WeaponType);

        //var currentAmmoInfos = unlockedWeapons[currentWeaponIndex].ammoInfos;
        //EventManager.OnAmmoConsumed?.Invoke(currentAmmoInfos.CurrentMagazineAmmo.ToString(), currentAmmoInfos.CurrentAmmoReserves.ToString());
    }

    public List<Weapon> Weapons()
    {
        return unlockedWeapons;
    }

    private void UnlockWeapon(WeaponType type)
    {
        var weaponToAdd = allWeapons.ToList().Find(w => w.WeaponType == type);
        if (!weaponToAdd.unlocked)
        {

            weaponToAdd.unlocked = true;
            unlockedWeapons.Add(weaponToAdd);

            if (type == WeaponType.Minigun)
            {
                Player.OnMeshChanged?.Invoke(type);
                GameManager.Instance.PreviousWeapon = unlockedWeapons[currentWeaponIndex];
                StartCoroutine(GameManager.Instance.MinigunUnlocked());
            }
        }
    }

    private void OnEnable()
    {
        PickUpWeapon.OnUnlockingWeapon += UnlockWeapon;
        UnlockedWeapons += Weapons;
        OnWeaponSwap += ChangeWeapon;
    }

    private void OnDisable()
    {
        UnlockedWeapons -= Weapons;
        OnWeaponSwap -= ChangeWeapon;
    }

}
