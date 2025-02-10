using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text postWaveInfo;
    [SerializeField] private Button restartButton;

    [Header("Handgun")]
    [SerializeField] private TextMeshProUGUI H_magazineText;
    [SerializeField] private TextMeshProUGUI H_reservesText;

    [Header("AssaultRifle")]
    [SerializeField] private TextMeshProUGUI AR_magazineText;
    [SerializeField] private TextMeshProUGUI AR_reservesText;

    [Header("Shotgun")]
    [SerializeField] private TextMeshProUGUI SH_magazineText;
    [SerializeField] private TextMeshProUGUI SH_reservesText;

    [Header("Sniper")]
    [SerializeField] private TextMeshProUGUI SN_magazineText;
    [SerializeField] private TextMeshProUGUI SN_reservesText;

    [Header("Hp and armor")]
    [SerializeField] private Image[] hpImages;
    [SerializeField] private Image[] armorImages;

    [Header("Grenades")]
    [SerializeField] private Image[] grenadeImages;

    private void OnEnable()
    {
        EventManager.OnAmmoConsumed += OnAmmoConsumed;
        EventManager.IncreaseScore += IncreaseScore;
        EventManager.HealthPlayerChange += PlayerHpChange;
        EventManager.ArmorPlayerChange += PlayerArmorChange;
        EventManager.GameOverEvent += GameOver;
        EventManager.OnGrenadeChange += OnGrenadeChanged;
        EventManager.OnCurrencyChange += CurrencyChange;
        EventManager.OnWaveChange += WaveChange;
        EventManager.OnStartWave += OnStartWave;
        EventManager.OnEndWave += OnEndWave;
    }


    private void OnDisable()
    {
        EventManager.OnAmmoConsumed -= OnAmmoConsumed;
        EventManager.IncreaseScore -= IncreaseScore;
        EventManager.HealthPlayerChange -= PlayerHpChange;
        EventManager.ArmorPlayerChange -= PlayerArmorChange;
        EventManager.GameOverEvent -= GameOver;
        EventManager.OnGrenadeChange -= OnGrenadeChanged;
        EventManager.OnCurrencyChange -= CurrencyChange;
        EventManager.OnWaveChange -= WaveChange;
        EventManager.OnStartWave -= OnStartWave;
        EventManager.OnEndWave -= OnEndWave;
    }

    private void IncreaseScore(int score)
    {
        scoreText.text = (int.Parse(scoreText.text) + score).ToString();
    }

    private void PlayerHpChange(float hp)
    {
        switch ((int)hp)
        {
            case 0:
                foreach (Image i in hpImages)
                    i.color = Color.black;
                break;
            case 1:
                hpImages[(int)hp - 1].color = Color.white;
                for(int i = 1; i < hpImages.Length; i++)
                    hpImages[i].color = Color.black;
                break;
            case 2:
                for(int i = 0; i < (int)hp; i++)
                    hpImages[i].color = Color.white;
                for (int i = (int)hp; i < hpImages.Length; i++)
                    hpImages[i].color = Color.black;
                break;
            case 3:
                for (int i = 0; i < (int)hp; i++)
                    hpImages[i].color = Color.white;
                for (int i = (int)hp; i < hpImages.Length; i++)
                    hpImages[i].color = Color.black;
                break;
            case 4:
                for (int i = 0; i < hpImages.Length - 1; i++)
                    hpImages[i].color = Color.white;
    
                hpImages[hpImages.Length - 1].color = Color.black;
                break;
            case 5:
                foreach (Image i in hpImages)
                    i.color = Color.white;
                break;
        }
    }

    private void PlayerArmorChange(int armor)
    {
        switch (armor)
        {
            case 0:
                foreach (Image i in armorImages)
                    i.color = Color.black;
                break;
            case 1:
                armorImages[armor - 1].color = Color.white;
                for (int i = 1; i < armorImages.Length; i++)
                    armorImages[i].color = Color.black;
                break;
            case 2:
                for (int i = 0; i < armor; i++)
                    armorImages[i].color = Color.white;
                for (int i = armor; i < armorImages.Length; i++)
                    armorImages[i].color = Color.black;
                break;
            case 3:
                for (int i = 0; i < armor; i++)
                    armorImages[i].color = Color.white;
                for (int i = armor; i < armorImages.Length; i++)
                    armorImages[i].color = Color.black;
                break;
            case 4:
                for (int i = 0; i < armorImages.Length - 1; i++)
                    armorImages[i].color = Color.white;

                armorImages[armorImages.Length - 1].color = Color.black;
                break;
            case 5:
                foreach (Image i in armorImages)
                    i.color = Color.white;
                break;
        }
    }

    public void OnEndWave()
    {
        postWaveInfo.gameObject.SetActive(true);
    }

    public void OnStartWave()
    {
        postWaveInfo.gameObject.SetActive(false);
    }

    private void WaveChange(int wave)
    {
        waveText.text = wave.ToString();
    }

    private void CurrencyChange(float currency)
    {
        currencyText.text = MathF.Round(currency,2).ToString();
    }

    public void OnAmmoConsumed(WeaponType weaponType, string currentAmmo, string currentReserves)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                H_magazineText.text = currentAmmo;
                H_reservesText.text = currentReserves;
                break;
            case WeaponType.AutoRifle:
                AR_magazineText.text = currentAmmo;
                AR_reservesText.text = currentReserves;
                break;
            case WeaponType.Shotgun:
                SH_magazineText.text = currentAmmo;
                SH_reservesText.text = currentReserves;
                break;
            case WeaponType.Sniper:
                SN_magazineText.text = currentAmmo;
                SN_reservesText.text = currentReserves;
                break;
        }
    }

    public void OnGrenadeChanged(int currentGrenade)
    {
        switch (currentGrenade)
        {
            case 0:
                foreach (Image i in grenadeImages)
                    i.color = Color.black;
                break;
            case 1:
                grenadeImages[currentGrenade - 1].color = Color.white;
                for (int i = 1; i < grenadeImages.Length; i++)
                    grenadeImages[i].color = Color.black;
                break;
            case 2:
                for (int i = 0; i < currentGrenade; i++)
                    grenadeImages[i].color = Color.white;
                for (int i = currentGrenade; i < grenadeImages.Length; i++)
                    grenadeImages[i].color = Color.black;
                break;
        }
    }

    private void GameOver()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
