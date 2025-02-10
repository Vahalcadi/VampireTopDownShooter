public class EnemyWithPistol : Enemy
{
    public EnemyWithPistolIdleState IdleState { get; private set; }
    public EnemyWithPistolBattleState BattleState { get; private set; }
    public EnemyWithPistolAttackState AttackState { get; private set; }
    // public EnemyWithPistolDamagedState DamagedState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new EnemyWithPistolIdleState(this, StateMachine, "Idle", this);
        BattleState = new EnemyWithPistolBattleState(this, StateMachine, "Move", this);
        AttackState = new EnemyWithPistolAttackState(this, StateMachine, "Shoot", this);
        //DamagedState = new EnemyWithPistolDamagedState(this, stateMachine, "Damaged", this);
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

}
