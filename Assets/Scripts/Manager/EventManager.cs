using System;

public static class EventManager
{
    public static Action<string> StopSpawn;
    public static Action StopAllSpawns;
    public static Action<int> IncreaseScore;
    public static Action<float> HealthPlayerChange;
    public static Action<int> ArmorPlayerChange;
    public static Action GameOverEvent;
    public static Action<WeaponType,string,string> OnAmmoConsumed;
    public static Action<int> OnGrenadeChange;
    public static Action<int> OnWaveChange;
    public static Action<float> OnCurrencyChange;
    public static Action OnEndWave;
    public static Action OnStartWave;
}
