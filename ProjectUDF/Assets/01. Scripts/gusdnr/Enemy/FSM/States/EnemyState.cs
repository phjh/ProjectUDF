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

	public abstract EnemyState Clone(); // ��ӹ޴� Ŭ�������� �������̵��Ͽ� ����

	// Clone �޼��带 ȣ���� �� �⺻ �������� Instantiate(this)�� ����Ͽ� ScriptableObject�� �����մϴ�.
	protected EnemyState CloneBase()
	{
		EnemyState cloneState = Instantiate(this);
		cloneState.Initialize(enemy, enemyStateMachine);
		return cloneState;
	}
}
