using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player : Entity
{
    public static Action<WeaponType> OnMeshChanged;
    public static Func<float> OnMaxHealth;
    public static Func<float> OnCurrentGrenadeInInventory;
    public static Func<float> OnMaxArmor;
    public static Func<float> OnCurrentArmor;

    public static Action<float> OnHealPlayer;
    public static Action<int> OnPlayerAddingArmor;

    public Vector3 MoveInput { get; set; }
    public InputManager InputManager { get; set; }
    private float attackCooldownTimer;
    private float shootCooldownTimer;

    [Header("Damaged VFX")]
    public Material defaultMat;
    public Material redMat;
    public VisualEffect invincibilityVFX;


    [Header("ArmorStats")]
    [SerializeField] private int maxArmor;
    public int CurrentArmor { get; set; }

    [Header("Dash Collision Infos")]
    public LayerMask maskToExclude;
    public LayerMask defaultExclude;

    [Header("Meshes")]
    public List<PlayerMeshChanger> meshChangerList;
    private GameObject currentMesh;

    [Header("Animator")]
    public Animator weaponAnim;
    [SerializeField] private float attackCooldown;

    [Header("Movement")]
    [SerializeField] private float aimedSpeed;
    [SerializeField] private int maxDashUses;
    [HideInInspector] public int dashUses;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    [HideInInspector] public float dashCooldownTimer;

    [Header("Inventory")]
    public float GrenadeCooldown;
    [HideInInspector] public float GrenadeCooldownTimer;
    [SerializeField] private GameObject GrenadePrefab;
    public int maxGrenadeInInventory;
    private int GrenadeInInventory;


    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerShootState ShootState { get; private set; }
    public PlayerMeleeAttackState MeleeAttackState { get; private set; }
    public PlayerAimState AimState { get; private set; }
    public PlayerReloadState ReloadState { get; private set; }
    public PlayerGrenadeState GrenadeState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        DashState = new PlayerDashState(this, StateMachine, "Idle");
        ShootState = new PlayerShootState(this, StateMachine, "Shoot");
        MeleeAttackState = new PlayerMeleeAttackState(this, StateMachine, "Attack");
        AimState = new PlayerAimState(this, StateMachine, "Move");
        ReloadState = new PlayerReloadState(this, StateMachine, "Reload");
        GrenadeState = new PlayerGrenadeState(this, StateMachine, "Grenade");

        currentMesh = meshChangerList[0].value;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InputManager = InputManager.Instance;
        CurrentArmor = 3;
        GrenadeInInventory = 1;
        EventManager.HealthPlayerChange?.Invoke(CurrentHP);
        EventManager.ArmorPlayerChange?.Invoke(CurrentArmor);
        EventManager.OnGrenadeChange?.Invoke(GrenadeInInventory);
        StateMachine.Initialize(IdleState);
        dashUses = maxDashUses;

        
    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        Look();

        dashCooldownTimer -= Time.deltaTime;
        attackCooldownTimer -= Time.deltaTime;
        shootCooldownTimer -= Time.deltaTime;
        GrenadeCooldownTimer -= Time.deltaTime;

        if (dashUses <= 0)
        {
            dashUses = maxDashUses;
            dashCooldownTimer = dashCooldown;

            //HUDManager.Instance.dashImages.ForEach(di => StartCoroutine(HUDManager.Instance.CheckCooldownOf(di, dashCooldown)));
        }

        StateMachine.CurrentState.Update();
        CheckForDashInput();

    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {

        if (InputManager.Dash() && dashCooldownTimer < 0)
        {
            if (dashUses > 0)
            {

                StateMachine.ChangeState(DashState);
            }
        }
    }
    public void HealPlayer(int health)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + health, 0, base.MaxHealth);
        EventManager.HealthPlayerChange?.Invoke(CurrentHP);
    }

    public void GainArmor(int armor)
    {
        CurrentArmor = Mathf.Clamp(CurrentArmor + armor, 0, maxArmor);
        EventManager.ArmorPlayerChange?.Invoke(CurrentArmor);
    }

    public override void TakeDamage(int damage)
    {
        if (IsInvincible)
            return;

        if (CurrentArmor > 0)
        {
            var difference = CurrentArmor - damage;
            CurrentArmor = Mathf.Clamp(difference, 0, maxArmor);
            /**
             * uncomment if player has to take damage if the damage recieved is greater than the current armor
             * **/
            /*if (CurrentArmor <= 0)
            {
                base.TakeDamage(difference);
                EventManager.HealthPlayerChange?.Invoke(CurrentHP);
            }*/
            EventManager.ArmorPlayerChange?.Invoke(CurrentArmor);
        }
        else
        {
            base.TakeDamage(damage);
            EventManager.HealthPlayerChange?.Invoke(CurrentHP);
        }

        if (CurrentHP == 0)
        {
            EventManager.GameOverEvent?.Invoke();
        }
        else
        {
            AudioManager.Instance.PlaySFX(16, transform);
            StartCoroutine(DamagedVFX(.5f));
            GameManager.Instance.MakeInvincible(.5f, this);
        }
        /*HUDManager.Instance.UpdateHealthBar();
        if (CurrentHP == 0)
            GameManager.Instance.EndGame("You Lost!, Play Again?");*/
    }

    private IEnumerator DamagedVFX(float duration)
    {
        GetComponentInChildren<MeshRenderer>().material = redMat;
        yield return new WaitForSeconds(duration);
        GetComponentInChildren<MeshRenderer>().material = defaultMat;
    }

    public void HealToFull()
    {
        CurrentHP = base.MaxHealth;
        EventManager.HealthPlayerChange?.Invoke(CurrentHP);
        //HUDManager.Instance.UpdateHealthBar();
    }

    public void Look()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.LookAt(mousePosition);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    public void SetAttackCooldownTimer()
    {
        attackCooldownTimer = attackCooldown;
    }

    public void SetShootCooldownTimer()
    {
        shootCooldownTimer = (float)Weapon.ShootCooldown?.Invoke();
    }

    public void SetGrenadeCooldownTimer()
    {
        GrenadeCooldownTimer = GrenadeCooldown;
    }

    public bool CanAttack()
    {
        if (attackCooldownTimer < 0)
            return true;

        return false;
    }

    public bool CanShoot()
    {
        if (shootCooldownTimer < 0)
            return true;

        return false;
    }

    public void Grenade()
    {
        if (GrenadeInInventory <= 0)
            return;
        Instantiate(GrenadePrefab, transform.position + transform.forward, transform.rotation);
        GrenadeInInventory--;
        EventManager.OnGrenadeChange?.Invoke(GrenadeInInventory);
    }

    public bool CanLaunchGrenade()
    {
        if (GrenadeCooldownTimer <= 0 && GrenadeInInventory > 0)
        {
            return true;
        }

        return false;
    }

    public bool TakeGrenade(int GrenadesNumber)
    {
        if (GrenadeInInventory == maxGrenadeInInventory)
        {
            return false;
        }

        GrenadeInInventory = Mathf.Clamp(GrenadeInInventory + GrenadesNumber, 0, maxGrenadeInInventory);
        EventManager.OnGrenadeChange?.Invoke(GrenadeInInventory);
        return true;
    }

    public void Reload()
    {
        AmmoInfos currentAmmoInfos = Weapon.AmmoInfos?.Invoke();
        WeaponType weaponType = (WeaponType)Weapon.OnWeaponType?.Invoke();

        if (currentAmmoInfos != null)
        {
            var ammoUntilMaxMag = currentAmmoInfos.MaxMagazine - currentAmmoInfos.CurrentMagazineAmmo;

            var ammoToAdd = Mathf.Clamp(currentAmmoInfos.CurrentAmmoReserves, currentAmmoInfos.CurrentAmmoReserves, currentAmmoInfos.MaxMagazine);

            currentAmmoInfos.CurrentAmmoReserves = Mathf.Clamp(currentAmmoInfos.CurrentAmmoReserves - ammoUntilMaxMag, 0, currentAmmoInfos.MaxReserves);

            currentAmmoInfos.CurrentMagazineAmmo = Mathf.Clamp(currentAmmoInfos.CurrentMagazineAmmo + ammoToAdd, currentAmmoInfos.CurrentMagazineAmmo, currentAmmoInfos.MaxMagazine);
            EventManager.OnAmmoConsumed?.Invoke(weaponType, currentAmmoInfos.CurrentMagazineAmmo.ToString(), currentAmmoInfos.CurrentAmmoReserves.ToString());
        }
    }

    public void HealPlayer(float health)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + health, CurrentHP, base.MaxHealth);
        EventManager.HealthPlayerChange?.Invoke(CurrentHP);
    }

    public void SwapWeapon(float scrollValue)
    {
        WeaponSwapper.OnWeaponSwap?.Invoke(scrollValue);
    }

    public void Movement()
    {
        /**
         * Two tipes of movement:
         * first one is dependant on the player rotation.
         * the second one is NOT dependant on the player rotation.
         * 
         * with the first type of movement, the movement axes always move around.
         * With the second type of movement, the movement axes are fixed in place.
         * **/

        Rb.velocity = Speed * Time.fixedDeltaTime * MoveInput.normalized;

        //rb.MovePosition(transform.position + (transform.rotation * playerMoveInput.normalized) * Speed * Time.fixedDeltaTime);
        //Rb.MovePosition(Speed * Time.fixedDeltaTime * MoveInput.normalized + transform.position);
    }

    public void AimedMovement()
    {
        Rb.velocity = aimedSpeed * Time.fixedDeltaTime * MoveInput.normalized;
        //Rb.MovePosition(aimedSpeed * Time.fixedDeltaTime * MoveInput.normalized + transform.position);

        CameraTransposer();
    }

    public void ReloadingMovement()
    {
        Rb.velocity = aimedSpeed * Time.fixedDeltaTime * MoveInput.normalized;
        //Rb.MovePosition(aimedSpeed * Time.fixedDeltaTime * MoveInput.normalized + transform.position);
    }

    private void CameraTransposer()
    {
        CinemachineVirtualCamera camera = PlayerFollowAssigner.GetVirtualCamera?.Invoke();
        Vector2 aimOffset = (Vector2)PlayerFollowAssigner.GetAimingOffset?.Invoke();

        float xAxis = Mathf.Clamp(camera.transform.position.x + Input.GetAxis("Mouse X"), transform.position.x - aimOffset.x, transform.position.x + aimOffset.x);
        float yAxis = Mathf.Clamp(camera.transform.position.z + Input.GetAxis("Mouse Y"), transform.position.z - aimOffset.y, transform.position.z + aimOffset.y);
        //camera.ForceCameraPosition(camera.transform.position + new Vector3(xAxis, 0, yAxis), camera.transform.rotation);
        camera.ForceCameraPosition(new Vector3(xAxis, camera.transform.position.y, yAxis), camera.transform.rotation);
    }

    private Transform ReturnPlayerTransform()
    {
        return transform;
    }
    private int MeleeWeapon_PlayerDamage()
    {
        return Damage;
    }

    private void PlayerMeshChanged(WeaponType _type)
    {
        GetComponentInChildren<MeshRenderer>().material = defaultMat;
        currentMesh.SetActive(false);
        currentMesh = meshChangerList.Find(go => go.key == _type).value;
        currentMesh.SetActive(true);

    }

    public void InvincibilityCoroutineVFX(float duration)
    {
        StartCoroutine(StartInvincibilityVFX(duration));
    }
    private IEnumerator StartInvincibilityVFX(float duration)
    {
        invincibilityVFX.Play();
        yield return new WaitForSeconds(duration);
        invincibilityVFX.Stop();
    }

    private float GetMaxHp()
    {
        return base.MaxHealth;
    }

    private float GetCurrentGrenade()
    {
        return GrenadeInInventory;
    }

    private float GetMaxArmor()
    {
        return maxArmor;
    }

    private float GetCurrentArmor()
    {
        return CurrentArmor;
    }

    private void OnEnable()
    {
        OnCurrentArmor += GetCurrentArmor;
        OnCurrentGrenadeInInventory += GetCurrentGrenade;
        OnHealPlayer += HealPlayer;
        OnPlayerAddingArmor += GainArmor;
        OnMaxArmor += GetMaxArmor; 
        OnMaxHealth += GetMaxHp;
        OnMeshChanged += PlayerMeshChanged;
        MeleeWeapon.PlayerDamage += MeleeWeapon_PlayerDamage;
        Enemy.OnPlayerPositionChanged += ReturnPlayerTransform;
        PlayerFollowAssigner.OnAssignPlayerAsFollowTarget += ReturnPlayerTransform;
    }

    private void OnDisable()
    {
        OnCurrentArmor -= GetCurrentArmor;
        OnCurrentGrenadeInInventory -= GetCurrentGrenade;
        OnHealPlayer -= HealPlayer;
        OnPlayerAddingArmor -= GainArmor;
        OnMaxArmor -= GetMaxArmor;
        OnMaxHealth -= GetMaxHp;
        OnMeshChanged -= PlayerMeshChanged;
        MeleeWeapon.PlayerDamage -= MeleeWeapon_PlayerDamage;
        Enemy.OnPlayerPositionChanged -= ReturnPlayerTransform;
        PlayerFollowAssigner.OnAssignPlayerAsFollowTarget -= ReturnPlayerTransform;
    }
}

[Serializable]
public class PlayerMeshChanger
{
    public WeaponType key;
    public GameObject value;
}
