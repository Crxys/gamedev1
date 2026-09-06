using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentEnemyState { get; set; }
    public void initialize(EnemyState startingState)
    {
        currentEnemyState = startingState;
        currentEnemyState.EnterState();
    }
    public void changeState(EnemyState newState)
    {
        currentEnemyState.ExitState();
        currentEnemyState = newState;
        currentEnemyState.EnterState();
    }
}
