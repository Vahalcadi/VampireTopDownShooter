using UnityEngine;
enum EnemyType
{
    Runner,
    Rat,
    Tank
}
public class Runner : Enemy
{
    [SerializeField] private EnemyType enemyType;
    public RunnerIdleState IdleState { get; private set; }
    public RunnerBattleState BattleState { get; private set; }
    public RunnerAttackState AttackState { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        IdleState = new RunnerIdleState(this, StateMachine, "Idle", this);
        BattleState = new RunnerBattleState(this, StateMachine, "Move", this);
        AttackState = new RunnerAttackState(this, StateMachine, "Shoot", this);
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    public override void AnimationAttackTrigger()
    {
        base.AnimationAttackTrigger();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !IsDead)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(Damage);
        }
    }

    public int GetSFXIndex()
    {
        int index = 0;
        switch (enemyType)
        {
            case EnemyType.Runner:
                index = 12;
                break;
            case EnemyType.Rat:
                index = 8;
                break;
            case EnemyType.Tank:
                index = 17;
                break;
        }

        return index;
    }

}
