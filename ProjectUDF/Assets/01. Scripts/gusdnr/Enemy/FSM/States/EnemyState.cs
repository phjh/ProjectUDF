using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
	public EnemyMain enemy;
	protected EnemyStateMachine enemyStateMachine;

	public virtual void Initialize(EnemyMain enemy, EnemyStateMachine stateMachine)
	{
		this.enemy = enemy;
		this.enemyStateMachine = stateMachine;
	}

	public virtual void EnterState() { }
	public virtual void ExitState() { }

	public virtual void FrameUpdate() { }
	public virtual void PhtsicsUpdate() { }

	public virtual void AnimationTriggerEvent(EnemyMain.AnimationTriggerType triggerType) { }
}
