using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
	protected EnemyMain enemy;
	protected EnemyStateMachine enemyStateMachine;

	public EnemyState(EnemyMain enemy, EnemyStateMachine stateMachine)
	{
		this.enemy = enemy;
		enemyStateMachine = stateMachine;
		Debug.Log(this.enemy.name);
	}

	public virtual void EnterState() { }
	public virtual void ExitState() { }

	public virtual void FrameUpdate() { }
	public virtual void PhtsicsUpdate() { }

	public virtual void AnimationTriggerEvent(EnemyMain.AnimationTriggerType triggerType) { }

	public EnemyState Clone() //적 패턴 SO를 복제 후, 돌려준다.
	{
		var returnvalue = Instantiate(this);
		return returnvalue;
	}
}
