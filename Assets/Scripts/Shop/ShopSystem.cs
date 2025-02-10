using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopSystem : SingletonA<ShopSystem>
{
    public GameObject[] powerUpScreens;
    public GameObject[] bloodIcons;
    private GameObject currentlyOpenPowerUpScreen;

    private Weapon WeaponToPowerUp;

    private float playerCurrency;
    public bool isShopOpen;
    [SerializeField] private GameObject Shop;
    [SerializeField] private TextMeshProUGUI playerCurrentCurrencyText;

    [Header("Handgun")]
    [SerializeField] private int HD_ammoCost;
    [SerializeField] private TextMeshProUGUI HD_ammoText;
    [SerializeField] private TextMeshProUGUI HD_DamageCostText;
    [SerializeField] private TextMeshProUGUI HD_PreviousDamageText;
    [SerializeField] private TextMeshProUGUI HD_NextDamageText;
    [SerializeField] private TextMeshProUGUI HD_ROFCostText;
    [SerializeField] private TextMeshProUGUI HD_PreviousROFText;
    [SerializeField] private TextMeshProUGUI HD_NextROFText;
    [SerializeField] private TextMeshProUGUI HD_MagCostText;
    [SerializeField] private TextMeshProUGUI HD_PreviousMagText;
    [SerializeField] private TextMeshProUGUI HD_NextMagText;
    [SerializeField] private TextMeshProUGUI HD_ReloadCostText;
    [SerializeField] private TextMeshProUGUI HD_PreviousReloadText;
    [SerializeField] private TextMeshProUGUI HD_NextReloadText;
    [SerializeField] private TextMeshProUGUI HD_ReservesCostText;
    [SerializeField] private TextMeshProUGUI HD_PreviousReservesText;
    [SerializeField] private TextMeshProUGUI HD_NextReservesText;


    [Header("AssaultRifle")]
    [SerializeField] private int AR_ammoCost;
    [SerializeField] private int AR_wpnCost;
    [SerializeField] private TextMeshProUGUI AR_ammoText;
    [SerializeField] private TextMeshProUGUI AR_wpnText;
    [SerializeField] private TextMeshProUGUI AR_DamageCostText;
    [SerializeField] private TextMeshProUGUI AR_PreviousDamageText;
    [SerializeField] private TextMeshProUGUI AR_NextDamageText;
    [SerializeField] private TextMeshProUGUI AR_ROFCostText;
    [SerializeField] private TextMeshProUGUI AR_PreviousROFText;
    [SerializeField] private TextMeshProUGUI AR_NextROFText;
    [SerializeField] private TextMeshProUGUI AR_MagCostText;
    [SerializeField] private TextMeshProUGUI AR_PreviousMagText;
    [SerializeField] private TextMeshProUGUI AR_NextMagText;
    [SerializeField] private TextMeshProUGUI AR_ReloadCostText;
    [SerializeField] private TextMeshProUGUI AR_PreviousReloadText;
    [SerializeField] private TextMeshProUGUI AR_NextReloadText;
    [SerializeField] private TextMeshProUGUI AR_ReservesCostText;
    [SerializeField] private TextMeshProUGUI AR_PreviousReservesText;
    [SerializeField] private TextMeshProUGUI AR_NextReservesText;

    [Header("Shotgun")]
    [SerializeField] private int SH_ammoCost;
    [SerializeField] private int SH_wpnCost;
    [SerializeField] private TextMeshProUGUI SH_ammoText;
    [SerializeField] private TextMeshProUGUI SH_wpnText;
    [SerializeField] private TextMeshProUGUI SH_DamageCostText;
    [SerializeField] private TextMeshProUGUI SH_PreviousDamageText;
    [SerializeField] private TextMeshProUGUI SH_NextDamageText;
    [SerializeField] private TextMeshProUGUI SH_ROFCostText;
    [SerializeField] private TextMeshProUGUI SH_PreviousROFText;
    [SerializeField] private TextMeshProUGUI SH_NextROFText;
    [SerializeField] private TextMeshProUGUI SH_MagCostText;
    [SerializeField] private TextMeshProUGUI SH_PreviousMagText;
    [SerializeField] private TextMeshProUGUI SH_NextMagText;
    [SerializeField] private TextMeshProUGUI SH_ReloadCostText;
    [SerializeField] private TextMeshProUGUI SH_PreviousReloadText;
    [SerializeField] private TextMeshProUGUI SH_NextReloadText;
    [SerializeField] private TextMeshProUGUI SH_ReservesCostText;
    [SerializeField] private TextMeshProUGUI SH_PreviousReservesText;
    [SerializeField] private TextMeshProUGUI SH_NextReservesText;

    [Header("Sniper")]
    [SerializeField] private int SN_ammoCost;
    [SerializeField] private int SN_wpnCost;
    [SerializeField] private TextMeshProUGUI SN_ammoText;
    [SerializeField] private TextMeshProUGUI SN_wpnText;
    [SerializeField] private TextMeshProUGUI SN_DamageCostText;
    [SerializeField] private TextMeshProUGUI SN_PreviousDamageText;
    [SerializeField] private TextMeshProUGUI SN_NextDamageText;
    [SerializeField] private TextMeshProUGUI SN_ROFCostText;
    [SerializeField] private TextMeshProUGUI SN_PreviousROFText;
    [SerializeField] private TextMeshProUGUI SN_NextROFText;
    [SerializeField] private TextMeshProUGUI SN_MagCostText;
    [SerializeField] private TextMeshProUGUI SN_PreviousMagText;
    [SerializeField] private TextMeshProUGUI SN_NextMagText;
    [SerializeField] private TextMeshProUGUI SN_ReloadCostText;
    [SerializeField] private TextMeshProUGUI SN_PreviousReloadText;
    [SerializeField] private TextMeshProUGUI SN_NextReloadText;
    [SerializeField] private TextMeshProUGUI SN_ReservesCostText;
    [SerializeField] private TextMeshProUGUI SN_PreviousReservesText;
    [SerializeField] private TextMeshProUGUI SN_NextReservesText;

    [Header("HP")]
    [SerializeField] private int HP_singleCost;
    [SerializeField] private int HP_allCost;
    [SerializeField] private TextMeshProUGUI HP_singleText;
    [SerializeField] private TextMeshProUGUI HP_allText;
    [SerializeField] private TextMeshProUGUI HP_currentText;

    [Header("Armor")]
    [SerializeField] private int AM_singleCost;
    [SerializeField] private int AM_allCost;
    [SerializeField] private TextMeshProUGUI AM_singleText;
    [SerializeField] private TextMeshProUGUI AM_allText;
    [SerializeField] private TextMeshProUGUI AM_currentText;

    [Header("Grenade")]
    [SerializeField] private int GD_singleCost;
    [SerializeField] private int GD_allCost;
    [SerializeField] private TextMeshProUGUI GD_singleText;
    [SerializeField] private TextMeshProUGUI GD_allText;
    [SerializeField] private TextMeshProUGUI GD_currentText;

    private void Start()
    {
        WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => w.WeaponType == WeaponType.Pistol).SetupModifiers();
    }

    public void Initialise()
    {
        Player player = PlayerFollowAssigner.OnAssignPlayerAsFollowTarget?.Invoke().gameObject.GetComponent<Player>();

        playerCurrency = GameManager.Instance.playerBloodCurrency;


        UpdatePlayerCurrencyText();
        HD_ammoText.text = $"All {HD_ammoCost}";
        AR_ammoText.text = $"All {AR_ammoCost}";
        SH_ammoText.text = $"All {SH_ammoCost}";
        SN_ammoText.text = $"All {SN_ammoCost}";

        InitialiseWeaponText();

        HP_singleText.text = $"Buy one {HP_singleCost}";
        HP_allText.text = $"Buy all {HP_allCost}";
        HP_currentText.text = $"{player.CurrentHP}/{player.MaxHealth}";

        AM_singleText.text = $"Buy one {AM_singleCost}";
        AM_allText.text = $"Buy all {AM_allCost}";
        AM_currentText.text = $"{player.CurrentArmor}/{Player.OnMaxArmor?.Invoke()}";

        GD_singleText.text = $"Buy one {GD_singleCost}";
        GD_allText.text = $"Buy all {GD_allCost}";
        GD_currentText.text = $"{Player.OnCurrentGrenadeInInventory?.Invoke()}/{player.maxGrenadeInInventory}";
    }

    private void InitialiseWeaponText()
    {
        AR_wpnText.text = $"Unlock {AR_wpnCost}";
        SH_wpnText.text = $"Unlock {SH_wpnCost}";
        SN_wpnText.text = $"Unlock {SN_wpnCost}";

        List<Weapon> weapons = WeaponSwapper.UnlockedWeapons?.Invoke();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.WeaponType)
            {
                case WeaponType.AutoRifle:
                    AR_wpnText.text = $"Unlocked";
                    break;
                case WeaponType.Shotgun:
                    SH_wpnText.text = $"Unlocked";
                    break;
                case WeaponType.Sniper:
                    SN_wpnText.text = $"Unlocked";
                    break;
            }
        }
    }


    public void UpdatePlayerCurrencyText()
    {
        GameManager.Instance.playerBloodCurrency = playerCurrency;
        EventManager.OnCurrencyChange?.Invoke(GameManager.Instance.playerBloodCurrency);
        playerCurrentCurrencyText.text = $"{MathF.Round(playerCurrency, 2)}";
    }

    public void TryGiveMaxAmmo(int weaponType)
    {
        try
        {
            int ammoCost = 0;
            Weapon weaponToGiveAmmo = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => (int)w.WeaponType == weaponType);
            switch (weaponType)
            {
                case (int)WeaponType.Pistol:
                    ammoCost = HD_ammoCost;
                    break;
                case (int)WeaponType.AutoRifle:
                    ammoCost = AR_ammoCost;
                    break;
                case (int)WeaponType.Shotgun:
                    ammoCost = SH_ammoCost;
                    break;
                case (int)WeaponType.Sniper:
                    ammoCost = SN_ammoCost;
                    break;
            }

            float difference = playerCurrency - ammoCost;

            if (difference < 0)
                throw new Exception("Sei un poveraccio ahahah");

            weaponToGiveAmmo.RefillReservesToMax();
            playerCurrency = difference;
            UpdatePlayerCurrencyText();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void TryGiveHp(int index)
    {
        try
        {
            Player player = PlayerFollowAssigner.OnAssignPlayerAsFollowTarget?.Invoke().gameObject.GetComponent<Player>();

            int cost = 0;
            int amount = 0;

            if (index == 0)
            {
                cost = HP_singleCost;
                amount = 1;
            }
            else
            {
                cost = HP_allCost;
                amount = (int)player.MaxHealth;
            }

            float difference = playerCurrency - cost;

            if (difference < 0)
                throw new Exception("Sei un poveraccio ahahah");

            player.HealPlayer(amount);

            HP_currentText.text = $"{player.CurrentHP}/{(int)player.MaxHealth}";

            playerCurrency = difference;
            UpdatePlayerCurrencyText();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void TryGiveArmor(int index)
    {
        try
        {
            Player player = PlayerFollowAssigner.OnAssignPlayerAsFollowTarget?.Invoke().gameObject.GetComponent<Player>();

            int cost = 0;
            int amount = 0;

            if (index == 0)
            {
                cost = AM_singleCost;
                amount = 1;
            }
            else
            {
                cost = AM_allCost;
                amount = (int)Player.OnMaxArmor?.Invoke();
            }

            float difference = playerCurrency - cost;

            if (difference < 0)
                throw new Exception("Sei un poveraccio ahahah");

            player.GainArmor(amount);
            AM_currentText.text = $"{player.CurrentArmor}/{(int)Player.OnMaxArmor?.Invoke()}";

            playerCurrency = difference;
            UpdatePlayerCurrencyText();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void TryGiveGrenade(int index)
    {
        try
        {
            Player player = PlayerFollowAssigner.OnAssignPlayerAsFollowTarget?.Invoke().gameObject.GetComponent<Player>();
            int cost = 0;
            int amount = 0;

            if (index == 0)
            {
                cost = GD_singleCost;
                amount = 1;
            }
            else
            {
                cost = GD_allCost;
                amount = player.maxGrenadeInInventory;
            }

            float difference = playerCurrency - cost;

            if (difference < 0)
                throw new Exception("Sei un poveraccio ahahah");

            player.TakeGrenade(amount);
            GD_currentText.text = $"{Player.OnCurrentGrenadeInInventory?.Invoke()}/{player.maxGrenadeInInventory}";
            playerCurrency = difference;
            UpdatePlayerCurrencyText();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }


    public void TryUnlockWeapons(int weaponType)
    {
        Weapon weaponToUnlock = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => (int)w.WeaponType == weaponType);

        try
        {
            int cost = 0;
            int index = 0;

            switch (weaponType)
            {
                case (int)WeaponType.AutoRifle:
                    cost = AR_wpnCost;
                    index = (int)WeaponType.AutoRifle;
                    break;
                case (int)WeaponType.Shotgun:
                    cost = SH_wpnCost;
                    index = (int)WeaponType.Shotgun;
                    break;
                case (int)WeaponType.Sniper:
                    cost = SN_wpnCost;
                    index = (int)WeaponType.Sniper;
                    break;
            }


            float difference = playerCurrency - cost;

            if (weaponToUnlock == null)
            {
                if (difference < 0)
                    throw new Exception("Sei un poveraccio ahahah");

                switch (weaponType)
                {
                    case (int)WeaponType.Pistol:
                        PickUpWeapon.OnUnlockingWeapon?.Invoke(WeaponType.Pistol);
                        weaponToUnlock = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => (int)w.WeaponType == weaponType);
                        weaponToUnlock.SetupModifiers();
                        EventManager.OnAmmoConsumed?.Invoke(weaponToUnlock.WeaponType, weaponToUnlock.ammoInfos.CurrentMagazineAmmo.ToString(), weaponToUnlock.ammoInfos.CurrentAmmoReserves.ToString());
                        break;
                    case (int)WeaponType.AutoRifle:
                        PickUpWeapon.OnUnlockingWeapon?.Invoke(WeaponType.AutoRifle);
                        weaponToUnlock = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => (int)w.WeaponType == weaponType);
                        weaponToUnlock.SetupModifiers();
                        AR_wpnText.text = $"Unlocked";
                        EventManager.OnAmmoConsumed?.Invoke(weaponToUnlock.WeaponType, weaponToUnlock.ammoInfos.CurrentMagazineAmmo.ToString(), weaponToUnlock.ammoInfos.CurrentAmmoReserves.ToString());
                        break;
                    case (int)WeaponType.Shotgun:
                        PickUpWeapon.OnUnlockingWeapon?.Invoke(WeaponType.Shotgun);
                        weaponToUnlock = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => (int)w.WeaponType == weaponType);
                        weaponToUnlock.SetupModifiers();
                        SH_wpnText.text = $"Unlocked";
                        EventManager.OnAmmoConsumed?.Invoke(weaponToUnlock.WeaponType, weaponToUnlock.ammoInfos.CurrentMagazineAmmo.ToString(), weaponToUnlock.ammoInfos.CurrentAmmoReserves.ToString());
                        break;
                    case (int)WeaponType.Sniper:
                        PickUpWeapon.OnUnlockingWeapon?.Invoke(WeaponType.Sniper);
                        weaponToUnlock = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => (int)w.WeaponType == weaponType);
                        weaponToUnlock.SetupModifiers();
                        SN_wpnText.text = $"Unlocked";
                        EventManager.OnAmmoConsumed?.Invoke(weaponToUnlock.WeaponType, weaponToUnlock.ammoInfos.CurrentMagazineAmmo.ToString(), weaponToUnlock.ammoInfos.CurrentAmmoReserves.ToString());
                        break;
                }

                playerCurrency = difference;
                UpdatePlayerCurrencyText();
                UpdatePowerUpMenuTexts();

            }
            else
            {
                OpenPowerupScreen(index);
                

            }


        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    public void OpenPowerupScreen(int index)
    {
        if (currentlyOpenPowerUpScreen != null)
            currentlyOpenPowerUpScreen.SetActive(false);    

        WeaponToPowerUp = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => (int)w.WeaponType == index);

        currentlyOpenPowerUpScreen = powerUpScreens[index];
        currentlyOpenPowerUpScreen.SetActive(true);

        UpdatePowerUpMenuTexts();
    }


    public void IncreaseDamage()
    {
        float difference = playerCurrency - WeaponToPowerUp.DamageModifierCost;

        if (difference < 0)
        {
            Debug.LogError("Sei un poveraccio ahahah");
            return;
        }
        else
        {
            WeaponToPowerUp.IncreaseDamage();
            playerCurrency = difference;
            UpdatePlayerCurrencyText();
            UpdatePowerUpMenuTexts();
        }
    }

    public void IncreaseRateOfFire()
    {
        float difference = playerCurrency - WeaponToPowerUp.RateOfFireModifierCost;

        if (WeaponToPowerUp.NumberRateOfFireApplied >= WeaponToPowerUp.maxRateOfFireModApplied)
            return;
        

        if (difference < 0)
        {
            Debug.LogError("Sei un poveraccio ahahah");
            return;
        }
        else
        {
            WeaponToPowerUp.IncreaseRateOfFire();
            playerCurrency = difference;
            WeaponToPowerUp.NumberRateOfFireApplied++;
            UpdatePlayerCurrencyText();
            UpdatePowerUpMenuTexts();
        }
    }

    public void IncreaseMagSize()
    {
        float difference = playerCurrency - WeaponToPowerUp.MagSizeModifierCost;

        if (difference < 0)
        {
            Debug.LogError("Sei un poveraccio ahahah");
            return;
        }
        else
        {
            WeaponToPowerUp.IncreaseMagSize();
            playerCurrency = difference;
            UpdatePlayerCurrencyText();
            UpdatePowerUpMenuTexts();
        }

    }

    public void IncreaseReloadSpeed()
    {
        float difference = playerCurrency - WeaponToPowerUp.ReloadSpeedModifierCost;

        if (WeaponToPowerUp.NumberReloadSpeedApplied >= WeaponToPowerUp.maxReloadSpeedModApplied)
            return;       

        if (difference < 0)
        {
            Debug.LogError("Sei un poveraccio ahahah");
            return;
        }
        else
        {
            WeaponToPowerUp.IncreaseReloadSpeed();
            playerCurrency = difference;
            WeaponToPowerUp.NumberReloadSpeedApplied++;
            UpdatePlayerCurrencyText();
            UpdatePowerUpMenuTexts();
        }
    }

    public void IncreaseReserves()
    {
        float difference = playerCurrency - WeaponToPowerUp.MaxAmmoModifierCost;

        if (difference < 0)
        {
            Debug.LogError("Sei un poveraccio ahahah");
            return;
        }
        else
        {
            WeaponToPowerUp.IncreaseReserves();
            playerCurrency = difference;
            UpdatePlayerCurrencyText();
            UpdatePowerUpMenuTexts();
        }

    }

    private void UpdatePowerUpMenuTexts()
    {
        switch (WeaponToPowerUp.WeaponType)
        {
            case WeaponType.Pistol:
                HD_DamageCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.DamageModifierCost, 2)}";
                HD_PreviousDamageText.text = $"{MathF.Round(WeaponToPowerUp.wpnDamage + WeaponToPowerUp.BulletPrefab.GetComponent<Projectile>().Damage,2)}";
                HD_NextDamageText.text = $"+{MathF.Round(WeaponToPowerUp.damageModifier, 2)}";

                HD_ROFCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.RateOfFireModifierCost, 2)}";
                HD_PreviousROFText.text = $"{MathF.Round(WeaponToPowerUp.shootCooldown, 2)}";
                HD_NextROFText.text = $"+{MathF.Round(WeaponToPowerUp.rateOfFireModifier, 2)}";

                HD_MagCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MagSizeModifierCost, 2)}";
                HD_PreviousMagText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxMagazine, 2)}";
                HD_NextMagText.text = $"+{MathF.Round(WeaponToPowerUp.magSizeModifier, 2)}";

                HD_ReloadCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.ReloadSpeedModifierCost, 2)}";
                HD_PreviousReloadText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.ReloadTime, 2)}";
                HD_NextReloadText.text = $"+{MathF.Round(WeaponToPowerUp.reloadSpeedModifier, 2)}";

                HD_ReservesCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MaxAmmoModifierCost, 2)}";
                HD_PreviousReservesText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxReserves, 2)}";
                HD_NextReservesText.text = $"+{MathF.Round(WeaponToPowerUp.maxAmmoModifier, 2)}";

                break;
            case WeaponType.AutoRifle:
                AR_DamageCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.DamageModifierCost, 2)}";
                Debug.Log(MathF.Round(WeaponToPowerUp.DamageModifierCost, 2));

                AR_PreviousDamageText.text = $"{MathF.Round(WeaponToPowerUp.wpnDamage + WeaponToPowerUp.BulletPrefab.GetComponent<Projectile>().Damage, 2)}";
                AR_NextDamageText.text = $"+{MathF.Round(WeaponToPowerUp.damageModifier, 2)}";
                
                AR_ROFCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.RateOfFireModifierCost, 2)}";
                AR_PreviousROFText.text = $"{MathF.Round(WeaponToPowerUp.shootCooldown, 2)}";
                AR_NextROFText.text = $"+{MathF.Round(WeaponToPowerUp.rateOfFireModifier, 2)}";
                
                AR_MagCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MagSizeModifierCost, 2)}";
                AR_PreviousMagText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxMagazine, 2)}";
                AR_NextMagText.text = $"+{MathF.Round(WeaponToPowerUp.magSizeModifier, 2)}";
                
                AR_ReloadCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.ReloadSpeedModifierCost, 2)}";
                AR_PreviousReloadText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.ReloadTime, 2)}";
                AR_NextReloadText.text = $"+{MathF.Round(WeaponToPowerUp.reloadSpeedModifier, 2)}";
                
                AR_ReservesCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MaxAmmoModifierCost, 2)}";
                AR_PreviousReservesText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxReserves, 2)}";
                AR_NextReservesText.text = $"+{MathF.Round(WeaponToPowerUp.maxAmmoModifier, 2)}";
                break;
            case WeaponType.Shotgun:
                SH_DamageCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.DamageModifierCost, 2)}";
                SH_PreviousDamageText.text = $"{MathF.Round(WeaponToPowerUp.wpnDamage + WeaponToPowerUp.BulletPrefab.GetComponent<Projectile>().Damage, 2)}";
                SH_NextDamageText.text = $"+{MathF.Round(WeaponToPowerUp.damageModifier, 2)}";
                
                SH_ROFCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.RateOfFireModifierCost, 2)}";
                SH_PreviousROFText.text = $"{MathF.Round(WeaponToPowerUp.shootCooldown, 2)}";
                SH_NextROFText.text = $"+{MathF.Round(WeaponToPowerUp.rateOfFireModifier, 2)}";
                
                SH_MagCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MagSizeModifierCost, 2)}";
                SH_PreviousMagText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxMagazine, 2)}";
                SH_NextMagText.text = $"+{MathF.Round(WeaponToPowerUp.magSizeModifier, 2)}";
                
                SH_ReloadCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.ReloadSpeedModifierCost, 2)}";
                SH_PreviousReloadText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.ReloadTime, 2)}";
                SH_NextReloadText.text = $"+{MathF.Round(WeaponToPowerUp.reloadSpeedModifier, 2)}";
                
                SH_ReservesCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MaxAmmoModifierCost, 2)}";
                SH_PreviousReservesText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxReserves, 2)}";
                SH_NextReservesText.text = $"+{MathF.Round(WeaponToPowerUp.maxAmmoModifier, 2)}";
                break;
            case WeaponType.Sniper:
                SN_DamageCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.DamageModifierCost,2)}";
                SN_PreviousDamageText.text = $"{MathF.Round(WeaponToPowerUp.wpnDamage + WeaponToPowerUp.BulletPrefab.GetComponent<Projectile>().Damage,2)}";
                SN_NextDamageText.text = $"+{MathF.Round(WeaponToPowerUp.damageModifier, 2)}";
                
                SN_ROFCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.RateOfFireModifierCost, 2)}";
                SN_PreviousROFText.text = $"{MathF.Round(WeaponToPowerUp.shootCooldown, 2)}";
                SN_NextROFText.text = $"+{MathF.Round(WeaponToPowerUp.rateOfFireModifier, 2)}";
                
                SN_MagCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MagSizeModifierCost, 2)}";
                SN_PreviousMagText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxMagazine, 2)}";
                SN_NextMagText.text = $"+{MathF.Round(WeaponToPowerUp.magSizeModifier, 2)}";
                
                SN_ReloadCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.ReloadSpeedModifierCost, 2)}";
                SN_PreviousReloadText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.ReloadTime, 2)}";
                SN_NextReloadText.text = $"+{MathF.Round(WeaponToPowerUp.reloadSpeedModifier, 2)}";
                
                SN_ReservesCostText.text = $"Buy {MathF.Round(WeaponToPowerUp.MaxAmmoModifierCost, 2)}";
                SN_PreviousReservesText.text = $"{MathF.Round(WeaponToPowerUp.ammoInfos.MaxReserves, 2)}";
                SN_NextReservesText.text = $"+{MathF.Round(WeaponToPowerUp.maxAmmoModifier, 2)}";
                break;

        }

        if (WeaponToPowerUp.NumberRateOfFireApplied >= WeaponToPowerUp.maxRateOfFireModApplied)
        {
            HD_ROFCostText.text = $"Maxed";
            AR_ROFCostText.text = $"Maxed";
            SH_ROFCostText.text = $"Maxed";
            SN_ROFCostText.text = $"Maxed";
            return;
        }
        if (WeaponToPowerUp.NumberReloadSpeedApplied >= WeaponToPowerUp.maxReloadSpeedModApplied)
        {
            HD_ReloadCostText.text = $"Maxed";
            AR_ReloadCostText.text = $"Maxed";
            SH_ReloadCostText.text = $"Maxed";
            SN_ReloadCostText.text = $"Maxed";
            return;
        }
    }

    public void GoBack()
    {
        Time.timeScale = 1.0f;
        Shop.SetActive(false);
        isShopOpen = false;
    }

    public void OpenCloseShop()
    {
        Shop.SetActive(!Shop.activeSelf);
        Initialise();

        if (Shop.activeSelf)
        {
            GameManager.Instance.SetCursor(false);
            Time.timeScale = 0;
        }
        else
        {
            GameManager.Instance.SetCursor(true);
            GoBack();
        }
    }

    private void OnEnable()
    {
    }
}
