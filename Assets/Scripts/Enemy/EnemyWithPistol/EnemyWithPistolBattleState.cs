using UnityEngine;

public class EnemyWithPistolBattleState : EnemyState
{
    private Transform player;
    private EnemyWithPistol enemy;

    public EnemyWithPistolBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyWithPistol _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = Enemy.OnPlayerPositionChanged?.Invoke();

        if (player == null)
            return;

        /*if (player.GetComponent<Player>().isDead)
            stateMachine.ChangeState(enemy.moveState);*/

        //AudioManager.instance.PlaySFX(49, enemy.transform);

    }

    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        /*if (enemy.IsDamaged)
            stateMachine.ChangeState(enemy.DamagedState);*/

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
