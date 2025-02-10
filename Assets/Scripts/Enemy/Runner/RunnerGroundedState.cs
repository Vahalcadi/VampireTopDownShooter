using UnityEngine;

public class RunnerGroundedState : EnemyState
{
    protected Runner enemy;
    protected Transform player;

    public RunnerGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Runner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = Enemy.OnPlayerPositionChanged?.Invoke();

        if (player == null)
            return;
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
    }
}
