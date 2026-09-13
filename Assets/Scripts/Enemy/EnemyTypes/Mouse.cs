using UnityEngine;

public class Mouse : Enemy
{
    public Mouse(Enemy enemy) : base(enemy)
    {
    }

    public new void FixedUpdate()
    {
        Debug.Log(enemy.RB);
        if (Physics2D.OverlapArea(new Vector2(enemy.RB.transform.position.x-3.5f,enemy.RB.transform.position.y+1.5f),new Vector2(enemy.RB.transform.position.x+10.5f,enemy.RB.transform.position.y-1.5f), enemy.player))
        {
            Debug.Log("change");
            enemy.enemyStateMachine.changeState(enemy.enemyDirectChase);
        }
    }
}
