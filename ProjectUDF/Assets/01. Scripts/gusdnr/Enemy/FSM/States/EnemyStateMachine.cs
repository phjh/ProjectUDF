using UnityEngine;

public class EnemyStateMachine
{
	public EnemyState CurrentState { get; private set; }

	public void Initialize(EnemyState startingState, EnemyMain enemy)
	{
		CurrentState = startingState;
		CurrentState?.EnterState();
	}

	public void ChangeState(EnemyState newState)
	{
		CurrentState?.ExitState();
		CurrentState = newState;
		CurrentState?.EnterState();
	}
}