using UnityEngine;

public class RunnerAttackState : EnemyState
{
    private Runner enemy;
    private int sfxIndex;

    public RunnerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Runner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.shootCooldown;
        enemy.SetMeleeAttack();
        sfxIndex = enemy.GetSFXIndex();

        AudioManager.Instance.PlaySFX(sfxIndex, enemy.transform);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.IsDamaged = false;
        enemy.ResetMeleeAttack();
        AudioManager.Instance.StopSFX(sfxIndex);

    }

    public override void Update()
    {
        base.Update();

        enemy.MoveToPlayer();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.BattleState);
    }
}
