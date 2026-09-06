using UnityEngine;

public class EnemyPacingIdle : EnemyState
{
    public EnemyPacingIdle(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }
    private int moveDirection = 1;
    private int moveSpeed = 3;

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.MoveEnemy(new Vector2(moveDirection * moveSpeed,enemy.RB.linearVelocityY));
        if (Physics2D.OverlapArea(new Vector2(enemy.RB.transform.position.x + 0.45f, enemy.RB.transform.position.y + 0.35f), new Vector2(enemy.RB.transform.position.x + 0.55f, enemy.RB.transform.position.y - 0.35f),enemy.ground))
        {
            moveDirection = -1;
        }
        if (Physics2D.OverlapArea(new Vector2(enemy.RB.transform.position.x - 0.45f, enemy.RB.transform.position.y + 0.35f), new Vector2(enemy.RB.transform.position.x - 0.55f, enemy.RB.transform.position.y - 0.35f), enemy.ground))
        {
            moveDirection = 1;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
