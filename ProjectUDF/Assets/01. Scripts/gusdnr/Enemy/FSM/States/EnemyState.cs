using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
	protected EnemyMain enemy;
	protected EnemyStateMachine enemyStateMachine;

	public virtual void Initialize(EnemyMain enemy, EnemyStateMachine stateMachine)
	{
		this.enemy = enemy;
		enemyStateMachine = stateMachine;
		Debug.Log($"Initialize Complete {enemy.name}");
	}

	public virtual void EnterState() { }
	public virtual void ExitState() { }

	public virtual void FrameUpdate() { }
	public virtual void PhtsicsUpdate() { }

	public virtual void AnimationTriggerEvent(EnemyMain.AnimationTriggerType triggerType) { }

	public abstract EnemyState Clone(); // 상속받는 클래스에서 오버라이드하여 구현

	// Clone 메서드를 호출할 때 기본 동작으로 Instantiate(this)를 사용하여 ScriptableObject을 복제합니다.
	protected EnemyState CloneBase()
	{
		EnemyState cloneState = Instantiate(this);
		cloneState.Initialize(enemy, enemyStateMachine);
		return cloneState;
	}
}
