using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmoBehaviour : MonoBehaviour
{

    [SerializeField] private WeaponType weaponType;
    [SerializeField] private int ammoToGive;

    private Weapon weaponToGiveAmmo;

    private void Start()
    {
        weaponToGiveAmmo = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => w.WeaponType == weaponType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (weaponToGiveAmmo == null)
                return;

            if (weaponToGiveAmmo.ammoInfos.CurrentAmmoReserves == weaponToGiveAmmo.ammoInfos.MaxReserves)
                return;

            var currentReserve = weaponToGiveAmmo.ammoInfos.CurrentAmmoReserves;
            var maxReserve = weaponToGiveAmmo.ammoInfos.MaxReserves;

            currentReserve = Mathf.Clamp(currentReserve + ammoToGive, currentReserve, maxReserve);

            weaponToGiveAmmo.ammoInfos.CurrentAmmoReserves = currentReserve;

            if(weaponToGiveAmmo.isActiveAndEnabled)
                EventManager.OnAmmoConsumed?.Invoke(weaponType, weaponToGiveAmmo.ammoInfos.CurrentMagazineAmmo.ToString(), weaponToGiveAmmo.ammoInfos.CurrentAmmoReserves.ToString());

            Destroy(gameObject);
        }
    }
}
