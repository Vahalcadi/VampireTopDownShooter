using UnityEngine;

public class EnemyWithPistolAttackState : EnemyState
{
    private EnemyWithPistol enemy;
    public EnemyWithPistolAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyWithPistol _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.shootCooldown;
        AudioManager.Instance.PlaySFX(13, enemy.transform);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
        enemy.IsDamaged = false;
        AudioManager.Instance.StopSFX(13);

    }

    public override void Update()
    {
        base.Update();

        enemy.MoveToPlayer();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.BattleState);
    }
}
