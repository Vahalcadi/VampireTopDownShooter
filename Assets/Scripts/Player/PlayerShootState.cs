public class PlayerShootState : PlayerState
{
    public PlayerShootState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Weapon.ShootAction?.Invoke();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetShootCooldownTimer();
    }

    public override void Update()
    {
        base.Update();

        //player.Look();
        player.Movement();

        if (triggerCalled)
            stateMachine.ChangeState(player.MoveState);
    }
}
