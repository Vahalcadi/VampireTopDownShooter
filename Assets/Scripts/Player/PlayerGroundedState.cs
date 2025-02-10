using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    float scrollValue;

    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.CanShoot() && player.InputManager.Shoot())
        {
            stateMachine.ChangeState(player.ShootState);
        }

        if ((scrollValue = player.InputManager.ChangeWeapon()) != 0)
        {
            player.SwapWeapon(scrollValue);
        }

        if (player.InputManager.Aim() && !(bool)Weapon.AmmoInfos?.Invoke().hasInfiniteAmmo)
        {
            stateMachine.ChangeState(player.AimState);
        }

        if (player.InputManager.Reload())
        {
            stateMachine.ChangeState(player.ReloadState);
        }

        if (player.CanAttack() && player.InputManager.MeleeAttack())
        {
            stateMachine.ChangeState(player.MeleeAttackState);
        }

        if (player.CanLaunchGrenade() && player.InputManager.Grenade())
        {
            stateMachine.ChangeState(player.GrenadeState);
        }
    }
}
