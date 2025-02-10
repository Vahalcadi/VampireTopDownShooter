using UnityEngine;

public class ReviverBattleState : EnemyState
{
    private Transform player;
    private Reviver enemy;

    public ReviverBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Reviver _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.FindNearestDeadEnemy();

        if (enemy.deadEnemy.Item1 == null) 
        {
            return;
        }

        if ((enemy.deadEnemy.Item1.transform.position - enemy.transform.position).magnitude < enemy.attackDistance)
        {
            if (CanShoot())
                stateMachine.ChangeState(enemy.AttackState);

        }
        enemy.LookAtDeadEnemies();
        enemy.MoveToDeadEnemies();
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
