using UnityEngine;

public class EnemyDirectChase : EnemyState
{
    private PlayerMovement player;
    private float moveSpeed = 4f;
    public EnemyDirectChase(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = Object.FindAnyObjectByType<PlayerMovement>();
    }

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
        float horizontal = Mathf.Sign(player.transform.position.x - enemy.RB.transform.position.x);
        enemy.RB.linearVelocityX = horizontal * moveSpeed;
        Debug.Log("hi");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
