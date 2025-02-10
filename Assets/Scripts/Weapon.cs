using System;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public enum WeaponType
{
    Pistol,
    AutoRifle,
    Shotgun,
    Sniper,
    Minigun,
    None
}

public class Weapon : MonoBehaviour
{
    public static Func<WeaponType> OnWeaponType;
    public static Action ShootAction;
    public static Func<float> ShootCooldown;
    public static Func<bool> Aiming;
    public static Func<AmmoInfos> AmmoInfos;
    public static Func<bool> CanShootRepeatedly;

    public WeaponType WeaponType;
    public GameObject BulletPrefab;
    [SerializeField] private Transform[] bulletSpawners;
    public float shootCooldown;
    public AmmoInfos ammoInfos;
    public bool unlocked;
    public bool canShootRepeatedly;

    [Header("VFXs")]
    [SerializeField] private VisualEffect fireEffect;

    [Header("set this only if the weapon has one single bullet spawner")]
    [SerializeField] private float recoilSway;
    [SerializeField] private float recoilWhenAiming;

    [Header("Increments")]
    public int damageModifier;
    [SerializeField] private float damageModifierCost;
    public float rateOfFireModifier;
    [SerializeField] private float rateOfFireModifierCost;
    public int magSizeModifier;
    [SerializeField] private float magSizeModifierCost;
    public float reloadSpeedModifier;
    [SerializeField] private float reloadSpeedModifierCost;
    public int maxAmmoModifier;
    [SerializeField] private float maxAmmoModifierCost;

    public int maxReloadSpeedModApplied;
    public int maxRateOfFireModApplied;

    public int NumberReloadSpeedApplied { get; set; }
    public int NumberRateOfFireApplied { get; set; }

    public float DamageModifierCost { get; set; }
    public float RateOfFireModifierCost { get; set; }
    public float MagSizeModifierCost { get; set; }
    public float ReloadSpeedModifierCost { get; set; }
    public float MaxAmmoModifierCost { get; set; }

    [HideInInspector]public int wpnDamage = 0;

    private void Awake()
    {
        ammoInfos = Instantiate(ammoInfos);

        ammoInfos.Initialise();
    }

    public void SetupModifiers()
    {
        DamageModifierCost = damageModifierCost;
        RateOfFireModifierCost = rateOfFireModifierCost;
        MagSizeModifierCost = magSizeModifierCost;
        ReloadSpeedModifierCost = reloadSpeedModifierCost;
        MaxAmmoModifierCost = maxAmmoModifierCost;
    }

    private void Start()
    {
        EventManager.OnAmmoConsumed?.Invoke(WeaponType, ammoInfos.CurrentMagazineAmmo.ToString(), ammoInfos.CurrentAmmoReserves.ToString());
    }

    public void RefillReservesToMax()
    {
        ammoInfos.CurrentAmmoReserves = ammoInfos.MaxReserves;
        EventManager.OnAmmoConsumed?.Invoke(WeaponType, ammoInfos.CurrentMagazineAmmo.ToString(), ammoInfos.CurrentAmmoReserves.ToString());

    }

    public void Shoot()
    {
        int audioIndex = 0;
        switch (WeaponType)
        {
            case WeaponType.Pistol:
                audioIndex = 0;
                break;
            case WeaponType.AutoRifle:
                audioIndex = 20;
                break;
            case WeaponType.Shotgun:
                audioIndex = 14;
                break;
            case WeaponType.Sniper:
                audioIndex = 15;
                break;
            case WeaponType.Minigun:
                audioIndex = 7;
                break;
        }

        float sway = AddRecoil();
        if (ammoInfos.CurrentMagazineAmmo <= 0)
        {
            AudioManager.Instance.PlaySFX(2, transform);
            return;
        }

        AudioManager.Instance.PlaySFX(audioIndex, transform);

        foreach (Transform t in bulletSpawners)
        {
            fireEffect.Play();
            GameObject go = Instantiate(BulletPrefab, t.position, Quaternion.Euler(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y + sway, t.rotation.eulerAngles.z));
            go.GetComponent<Projectile>().Damage += wpnDamage;
            PlayerFollowAssigner.CameraShake?.Invoke(ammoInfos.cameraShakeIntensity, ammoInfos.cameraShakeTime);
            Debug.Log($"Mag ammo: {ammoInfos.CurrentMagazineAmmo}");
        }
        if (!ammoInfos.hasInfiniteAmmo)
        {
            ammoInfos.CurrentMagazineAmmo--;
            EventManager.OnAmmoConsumed?.Invoke(WeaponType, ammoInfos.CurrentMagazineAmmo.ToString(), ammoInfos.CurrentAmmoReserves.ToString());
        }
        /*else
            EventManager.OnAmmoConsumed?.Invoke("∞", "∞");*/
    }

    private float AddRecoil()
    {
        if (bulletSpawners.Length == 1)
        {
            float sway = (bool)Aiming?.Invoke() ? UnityEngine.Random.Range(-recoilWhenAiming, recoilWhenAiming) : UnityEngine.Random.Range(-recoilSway, recoilSway);
            return sway;
        }

        return 0;
    }

    public AmmoInfos GetAmmoInfos()
    {
        return ammoInfos;
    }

    public float GetShootCooldown()
    {
        return shootCooldown;
    }

    public bool GetCanShootRepeatedly()
    {
        return canShootRepeatedly;
    }

    public WeaponType GetWeaponType()
    {
        return WeaponType;
    }

    public void IncreaseDamage()
    {
        wpnDamage += damageModifier;
        DamageModifierCost += damageModifierCost;
    }

    public void IncreaseRateOfFire()
    {
        shootCooldown -= rateOfFireModifier;
        RateOfFireModifierCost += rateOfFireModifierCost;
    }

    public void IncreaseMagSize()
    {
        ammoInfos.MaxMagazine += magSizeModifier;
        MagSizeModifierCost += magSizeModifierCost;
    }

    public void IncreaseReloadSpeed()
    {
        ammoInfos.ReloadTime -= reloadSpeedModifier;
        ReloadSpeedModifierCost += reloadSpeedModifierCost;
    }

    public void IncreaseReserves()
    {
        ammoInfos.MaxReserves += maxAmmoModifier;
        MaxAmmoModifierCost += maxAmmoModifierCost;
    }

    private void OnEnable()
    {
        OnWeaponType += GetWeaponType;
        CanShootRepeatedly += GetCanShootRepeatedly;
        AmmoInfos += GetAmmoInfos;
        ShootCooldown += GetShootCooldown;
        ShootAction += Shoot;
    }

    private void OnDisable()
    {
        OnWeaponType -= GetWeaponType;
        CanShootRepeatedly -= GetCanShootRepeatedly;
        AmmoInfos -= GetAmmoInfos;
        ShootCooldown -= GetShootCooldown;
        ShootAction -= Shoot;
    }
}
