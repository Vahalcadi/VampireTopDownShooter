public class Reviver : Enemy
{
    public ReviverIdleState IdleState { get; private set; }
    public ReviverBattleState BattleState { get; private set; }
    public ReviverReviveState AttackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new ReviverIdleState(this, StateMachine, "Idle", this);
        BattleState = new ReviverBattleState(this, StateMachine, "Move", this);
        AttackState = new ReviverReviveState(this, StateMachine, "Shoot", this);
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
