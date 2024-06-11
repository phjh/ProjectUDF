using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
	public EnemyMain.EnemyMotionState MotionState;

	protected EnemyMain enemy;
	protected EnemyStateMachine enemyStateMachine;

	public virtual void Initialize(EnemyMain enemy, EnemyStateMachine stateMachine)
	{
		this.enemy = enemy;
		enemyStateMachine = stateMachine;
	}

	public virtual void Initialize(EnemyMain enemy, EnemyStateMachine stateMachine, EnemyMain.EnemyMotionState MotionState)
	{
		this.enemy = enemy;
		enemyStateMachine = stateMachine;
		this.MotionState = MotionState;
	}

	public virtual void EnterState() { }
	public virtual void ExitState() { }

	public virtual void FrameUpdate() { }
	public virtual void PhysicsUpdate() { }

	public virtual void AnimationTriggerEvent(EnemyMain.AnimationTriggerType triggerType) { }

	public abstract EnemyState Clone(); // 상속받는 클래스에서 오버라이드하여 구현

	// Clone 메서드를 호출할 때 기본 동작으로 Instantiate(this)를 사용하여 ScriptableObject을 복제합니다.
	protected EnemyState CloneBase()
	{
		EnemyState cloneState = Instantiate(this);
		cloneState.Initialize(enemy, enemyStateMachine, MotionState);
		cloneState.name = "(Cloned)" + name.Replace("EnemyMotionState", "");
		return cloneState;
	}
}
