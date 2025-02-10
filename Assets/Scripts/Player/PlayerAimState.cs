public class PlayerAimState : PlayerGroundedState
{

    public PlayerAimState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        PlayerFollowAssigner.TurnOnOffVirtualCamera?.Invoke(false);
    }

    public override void Exit()
    {
        base.Exit();
        PlayerFollowAssigner.TurnOnOffVirtualCamera?.Invoke(true);
    }

    public override void Update()
    {
        base.Update();

        //player.Look();
        player.AimedMovement();

        if (!player.InputManager.Aim())
            stateMachine.ChangeState(player.MoveState);
    }
}
