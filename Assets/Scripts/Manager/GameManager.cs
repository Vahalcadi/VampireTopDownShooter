using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonA<GameManager>
{
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotspot;
    private Transform playerTransform;
    public static Action<int> OnReturningCost;

    [HideInInspector] public float playerBloodCurrency = 0;

    public int CurrentWave = 1;

    [SerializeField] private int enemiesToKeep;
    public List<GameObject> deadEnemies;

    [SerializeField] private int totalCost = 10;
    public int returningCost = 0;

    public int CurrentSpawnCurrency { get; set; }
    private int currentModifier = 0;

    private bool coroutineStarted;
    // It's ONLY used when the player picks up a Minigun
    public Weapon PreviousWeapon { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 1.0f;

        cursorHotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2.2f);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    private void Start()
    {
        SetCurrentModifier();
        SetNextTotalCost(currentModifier);
        playerTransform = PlayerFollowAssigner.OnAssignPlayerAsFollowTarget?.Invoke();

        EventManager.OnWaveChange?.Invoke(CurrentWave);
    }
    private void Update()
    {      
        if (!coroutineStarted && returningCost == totalCost)
            StartCoroutine(NextWave());

        if (InputManager.Instance.Interact() && returningCost == totalCost)
            ShopSystem.Instance.OpenCloseShop();
    }

    public void SetCursor(bool active)
    {
        if(active)
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private IEnumerator NextWave()
    {
        AudioManager.Instance.StopAllBGM();
        AudioManager.Instance.PlaySFX(3, playerTransform);
        coroutineStarted = true;
        EventManager.OnEndWave?.Invoke();
        yield return new WaitUntil(() => InputManager.Instance.SkipWave());
        ShopSystem.Instance.GoBack();
        SetCursor(true);
        CurrentWave++;
        EventManager.OnWaveChange?.Invoke(CurrentWave);
        EventManager.OnStartWave?.Invoke();
        SetCurrentModifier();
        SetNextTotalCost(currentModifier);
        AudioManager.Instance.StopSFX(3);
        AudioManager.Instance.PlayBGM(0);
        coroutineStarted = false;
    }

    private void SetNextTotalCost(int modifier)
    {
        totalCost += modifier;
        CurrentSpawnCurrency = totalCost;
        returningCost = 0;
    }

    public void AddEnemyToTheDeadList(GameObject deadEnemy)
    {
        deadEnemies.Add(deadEnemy);

        if (deadEnemies.Count > enemiesToKeep)
        {
            Destroy(deadEnemies[^1]);
            deadEnemies.RemoveAt(deadEnemies.Count - 1);
        }
    }

    public void MakeInvincible(float timer, Player player)
    {

        StartCoroutine(MakeInvincibleCO(timer, player));
    }

    public IEnumerator MinigunUnlocked()
    {
        PreviousWeapon.gameObject.SetActive(false);
        var weapon = WeaponSwapper.UnlockedWeapons?.Invoke().Find(w => w.WeaponType == WeaponType.Minigun);
        weapon.gameObject.SetActive(true);

        yield return new WaitForSeconds((float)weapon.ammoInfos.Duration);
        weapon.unlocked = false;
        weapon.gameObject.SetActive(false);

        PreviousWeapon.gameObject.SetActive(true);
        Player.OnMeshChanged?.Invoke(PreviousWeapon.WeaponType);

        PreviousWeapon = null;

        try
        {
            WeaponSwapper.UnlockedWeapons?.Invoke().Remove(weapon); //should be minigun
        }
        catch
        {
            Debug.LogException(new Exception("compà, non hai la minigun ahah"));
        }
    }

    public IEnumerator MakeInvincibleCO(float timer, Player player)
    {
        player.MakeInvincible(true);
        yield return new WaitForSeconds(timer);
        player.MakeInvincible(false);
    }

    private void SetCurrentModifier()
    {
        if (CurrentWave > 1 && CurrentWave <= 10)
            currentModifier = 3;
        else if (CurrentWave > 10 && CurrentWave <= 20)
            currentModifier = 6;
        else if (CurrentWave > 20 && CurrentWave <= 30)
            currentModifier = 11;
        else if (CurrentWave > 30 && CurrentWave <= 40)
            currentModifier = 27;
        else if (CurrentWave > 40 && CurrentWave <= 50)
            currentModifier = 60;
        else if (CurrentWave > 50)
            currentModifier = 150;
        else
            currentModifier = 0;
    }

    private void ReturningCost(int n)
    {
        returningCost += n;
    }

    private void OnEnable()
    {
        OnReturningCost += ReturningCost;
    }
    private void OnDisable()
    {
        OnReturningCost -= ReturningCost;
    }
}
