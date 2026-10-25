using UnityEngine;

public class Mouse : Enemy
{
    public Mouse(Enemy enemy) : base(enemy)
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public new void FixedUpdate()
    {
        if ((RB!=null) && Physics2D.OverlapArea(new Vector2(RB.transform.position.x-3.5f,RB.transform.position.y+1.5f),new Vector2(RB.transform.position.x+10.5f,RB.transform.position.y-1.5f), 8))
        {
            enemy.enemyStateMachine.changeState(enemy.enemyDirectChase);
        }
    }
}
