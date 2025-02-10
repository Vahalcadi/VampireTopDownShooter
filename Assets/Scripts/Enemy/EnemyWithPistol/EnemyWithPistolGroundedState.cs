using UnityEngine;

public class EnemyWithPistolGroundedState : EnemyState
{
    protected EnemyWithPistol enemy;
    protected Transform player;

    public EnemyWithPistolGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyWithPistol _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = Enemy.OnPlayerPositionChanged?.Invoke();

        if (player == null)
            return;

        //Debug.Log("1. GroundedState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        stateMachine.ChangeState(enemy.BattleState);

        /*if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.aggroDistance)
            stateMachine.ChangeState(enemy.battleState);*/
    }
}
