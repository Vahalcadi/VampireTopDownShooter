using UnityEngine;

public class PlayerDashState : PlayerState
{
    /*float inputMagnitude;
    Quaternion rotation;*/

    private LayerMask playerExludedMasks;

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (stateTimer > 0)
            return;

        //rotation = Quaternion.LookRotation(player.moveInput, Vector3.up);

        player.dashUses--;
        //HUDManager.Instance.SetCooldownOf(HUDManager.Instance.dashImages[player.dashUses]);
        stateTimer = player.dashDuration;

        player.GetComponent<CapsuleCollider>().excludeLayers = player.maskToExclude;

        player.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.GetComponent<CapsuleCollider>().excludeLayers = player.defaultExclude;
        player.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();


        if (player.MoveInput.normalized.magnitude == 0)
        {
            rb.velocity = player.transform.forward * player.dashSpeed;
        }
        else
        {
            rb.velocity = player.MoveInput.normalized * player.dashSpeed;
        }


        if (stateTimer < 0)
            stateMachine.ChangeState(player.IdleState);
    }
}
