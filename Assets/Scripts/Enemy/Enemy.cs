using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public static Func<Transform> OnPlayerPositionChanged;

    //[SerializeField] protected LayerMask whatIsPlayer;

    [Header("incrementals")]
    [SerializeField] private float hpIncrement;
    [SerializeField] private float speedIncrement;

    [Header("money to drop when killed")]
    [SerializeField] private float moneyToDropOnDeath;

    public bool IsRevived { get; set; }
    public bool IsDamaged { get; set; }
    public int UnitCost { get; set; }

    [Header("AI")]
    private NavMeshAgent agent;

    [Header("Weapon")]
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform bulletSpawner;
    public float shootCooldown;

    /*[Header("VFX")]
    public ParticleSystem damageTakenEffect;
    public VisualEffect damageTakenEffect1;
    public VisualEffect attackVFX;
    private Material material;
    private bool hasDamageCoroutineStarted;*/

    /*[Header("Stunned info")]
    public float stunCooldown = 1;
    protected float stunCooldownTimer;*/


    [Header("Attack info")]
    public float attackDistance = 2;
    /*public float minAttackCooldown = .35f;
    public float maxAttackCooldown = .55f;*/
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine StateMachine { get; private set; }
    public string LastAnimBoolName { get; private set; }
    public (GameObject, float) deadEnemy;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();
        SetIncrementedStats();
    }

    protected override void Start()
    {
        base.Start();
        agent.speed = Speed;
    }

    protected override void Update()
    {
        base.Update();
        //stunCooldownTimer = Time.deltaTime;
        StateMachine.CurrentState.Update();
    }

    public override void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        base.TakeDamage(damage);
        //damageTakenEffect1.Play();

        /*if (this.isActiveAndEnabled)
            StartCoroutine(ShowDamageVFX());*/
        IsDamaged = true;
    }

    public override void Die()
    {
        base.Die();
        agent.enabled = false;

        if (!IsRevived)
        {
            GameManager.OnReturningCost?.Invoke(UnitCost);
            GameManager.Instance.playerBloodCurrency += moneyToDropOnDeath;
            EventManager.OnCurrencyChange?.Invoke(GameManager.Instance.playerBloodCurrency);
        }
    }

    public override void ReviveEntity()
    {
        base.ReviveEntity();
        agent.enabled = true;
    }

    public void Shoot()
    {
        Instantiate(BulletPrefab, bulletSpawner.position, bulletSpawner.rotation);
    }

    #region Runner

    public void MeleeAttack()
    {

    }

    public void SetMeleeAttack()
    {
        agent.speed *= 5;
        agent.stoppingDistance = 2;
    }

    public void ResetMeleeAttack()
    {
        agent.speed /= 5;
        agent.stoppingDistance = attackDistance;
    }

    #endregion

    /*private IEnumerator ShowDamageVFX()
    {
        if (hasDamageCoroutineStarted)
            yield return null;

        material = GetComponentInChildren<Renderer>().material;
        hasDamageCoroutineStarted = true;
        material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        material.color = Color.white;
        hasDamageCoroutineStarted = false;
    }*/

    /*public virtual void GetStunned()
    {
        stunCooldownTimer = stunCooldown;
    }*/

    public override void SetZeroVelocity()
    {
        base.SetZeroVelocity();
        agent.velocity = Vector3.zero;

    }

    public void MoveToPlayer()
    {
        Vector3? playerPos = OnPlayerPositionChanged?.Invoke().position;
        if (playerPos == null)
            return;

        Vector3 pos = (Vector3)playerPos;
        agent.SetDestination(new Vector3(pos.x, transform.position.y, pos.z));
    }

    public void LookAtPlayer()
    {
        Vector3 playerPos = (Vector3)OnPlayerPositionChanged?.Invoke().position;
        transform.LookAt(playerPos);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

    #region Reviver

    public void FindNearestDeadEnemy()
    {
        if (GameManager.Instance.deadEnemies.Count == 0)
        {
            return;
        }

        deadEnemy = (GameManager.Instance.deadEnemies[0], float.MaxValue);
        foreach (var enemy in GameManager.Instance.deadEnemies)
        {
            if ((transform.position - enemy.transform.position).magnitude <= (transform.position - deadEnemy.Item1.transform.position).magnitude)
            {
                deadEnemy = (enemy, (transform.position - enemy.transform.position).magnitude);
            }
        }
    }

    public void MoveToDeadEnemies()
    {
        if (deadEnemy.Item1 == null)
            return;

        Vector3 pos = deadEnemy.Item1.transform.position;
        agent.SetDestination(new Vector3(pos.x, transform.position.y, pos.z));
    }

    public void LookAtDeadEnemies()
    {
        if (deadEnemy.Item1 == null)
            return;

        transform.LookAt(deadEnemy.Item1.transform.position);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

    public void Revive()
    {
        if (deadEnemy.Item1 == null)
            return;

        if (deadEnemy.Item2 <= attackDistance)
        {
            deadEnemy.Item1.GetComponent<Enemy>().ReviveMe();
            GameManager.Instance.deadEnemies.Remove(deadEnemy.Item1);
        }

        deadEnemy = (null, float.MaxValue);
    }

    public void ReviveMe()
    {
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.ReviveEntity();
        enemy.CurrentHP = enemy.MaxHealth;
        IsRevived = true;
    }

    #endregion

    /*public virtual bool CanBeStunned()
    {
        if (stunCooldownTimer < 0)
            return true;
        return false;
    }*/
    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public virtual void AnimationAttackTrigger() { }

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        LastAnimBoolName = _animBoolName;
    }

    private void SetIncrementedStats()
    {
        int currentWave = GameManager.Instance.CurrentWave;

        if (currentWave < 50 && currentWave > 0)
        {
            HpIncrement(hpIncrement);
            SpeedIncrement(speedIncrement);
        }
        else if (currentWave >= 50)
            HpIncrement(hpIncrement);
    }

    private void HpIncrement(float incremental)
    {
        MaxHealth *= incremental;
        CurrentHP = MaxHealth;

    }
    private void SpeedIncrement(float incremental)
    {
        Speed *= incremental;

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, transform.position + (transform.rotation * new Vector3(0, 0, attackDistance)));
    }
}
