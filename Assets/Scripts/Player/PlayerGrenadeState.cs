public class PlayerGrenadeState : PlayerState
{
    public PlayerGrenadeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Grenade();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGrenadeCooldownTimer();
    }

    public override void Update()
    {
        base.Update();

        player.Movement();

        if (triggerCalled)
            stateMachine.ChangeState(player.MoveState);
    }
}
