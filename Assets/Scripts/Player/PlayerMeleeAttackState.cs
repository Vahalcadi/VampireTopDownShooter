using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackState : PlayerState
{
    public PlayerMeleeAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
        player.SetAttackCooldownTimer();
    }

    public override void Update()
    {
        base.Update();

        player.Movement();

        /**
         * dashes forward when attacking
         * **/
        /*if (stateTimer < 0)
        {
            rb.MovePosition(player.transform.position + player.transform.forward * (player.Speed / 4) * Time.fixedDeltaTime);
        }*/

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
