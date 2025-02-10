using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public static Action<WeaponType> OnUnlockingWeapon;

    [SerializeField] private WeaponType weaponType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnUnlockingWeapon?.Invoke(weaponType);
            Destroy(gameObject);
        }
    }


}
