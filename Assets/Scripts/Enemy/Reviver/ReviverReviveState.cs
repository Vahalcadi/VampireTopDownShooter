using UnityEngine;

public class ReviverReviveState : EnemyState
{
    private Reviver enemy;

    public ReviverReviveState(Reviver _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Reviver _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.shootCooldown;
        AudioManager.Instance.PlaySFX(11, enemy.transform);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.IsDamaged = false;
        AudioManager.Instance.StopSFX(11);
    }

    public override void Update()
    {
        base.Update();

        enemy.MoveToDeadEnemies();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.BattleState);
    }
}
