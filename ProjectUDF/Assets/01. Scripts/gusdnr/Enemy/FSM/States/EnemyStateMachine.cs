using UnityEngine;

public class EnemyStateMachine
{
	public EnemyState CurrentState { get; private set; }

	public void Initialize(EnemyState startingState, EnemyMain enemy)
    {
        Debug.Log("123");
        startingState.Initialize(enemy, this);
		CurrentState = startingState;
		CurrentState?.EnterState();
	}

	public void ChangeState(EnemyState newState)
	{
		CurrentState?.ExitState();
		newState.Initialize(CurrentState.enemy, this);
		CurrentState = newState;
		CurrentState?.EnterState();
	}
}