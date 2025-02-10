using UnityEngine;

public class RunnerBattleState : EnemyState
{
    private Transform player;
    private Runner enemy;


    public RunnerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Runner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        if ((player.position - enemy.transform.position).magnitude < enemy.attackDistance)
        {
            if (CanShoot())
                stateMachine.ChangeState(enemy.AttackState);

        }
        enemy.LookAtPlayer();
        enemy.MoveToPlayer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanShoot()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.shootCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
