using UnityEngine;

public class ReviverGroundedState : EnemyState
{
    protected Reviver enemy;
    protected Transform player;

    public ReviverGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Reviver _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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

        stateMachine.ChangeState(enemy.BattleState);
    }
}
