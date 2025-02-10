using UnityEngine;

[CreateAssetMenu]
public class AmmoInfos : ScriptableObject
{
    [SerializeField] private int maxMagazine;
    public bool hasInfiniteAmmo;
    [SerializeField] private float reloadTime;

    [Header("Camera Shake values")]
    public float cameraShakeIntensity;
    public float cameraShakeTime;

    [Header("Set only if hasInfiniteAmmo is set to true")]
    [SerializeField] private float duration;

    private int maxReserves;

    public int MaxMagazine { get => maxMagazine; set => maxMagazine = value; }
    public int MaxReserves { get => maxReserves; set => maxReserves = value; }
    public float Duration { get => duration; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public int CurrentMagazineAmmo { get; set; }
    public int CurrentAmmoReserves { get; set; }

    public void Initialise()
    {
        maxReserves = maxMagazine * 5;

        CurrentAmmoReserves = MaxReserves;
        CurrentMagazineAmmo = maxMagazine;

        CurrentAmmoReserves -= CurrentMagazineAmmo;
    }
}
