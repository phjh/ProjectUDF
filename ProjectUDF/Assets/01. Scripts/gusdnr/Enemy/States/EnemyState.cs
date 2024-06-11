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

	public abstract EnemyState Clone(); // ��ӹ޴� Ŭ�������� �������̵��Ͽ� ����

	// Clone �޼��带 ȣ���� �� �⺻ �������� Instantiate(this)�� ����Ͽ� ScriptableObject�� �����մϴ�.
	protected EnemyState CloneBase()
	{
		EnemyState cloneState = Instantiate(this);
		cloneState.Initialize(enemy, enemyStateMachine, MotionState);
		cloneState.name = "(Cloned)" + name.Replace("EnemyMotionState", "");
		return cloneState;
	}
}
