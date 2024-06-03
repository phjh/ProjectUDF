using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_ShockWave", menuName = "SO/Boss/KingToad/Pattern_ShockWave")]
public class PatternShockWave : BossPattern
{
	[SerializeField] private int RepeatCount = 4;
	[SerializeField] private float AttackTerm = 0.5f;
	[SerializeField] private EnemyProjectile[] ShockWaveZones;
	
	private int attackCount = 0;
	private List<int> AttackPoses;
	private Coroutine AttackCoroutine = null;

	public override void Initialize(BossMain bossMain)
	{
		ResetValues();
	}

	public override void EnterPattern()
	{
		ResetValues();

	}

	public override void ActivePattern()
	{
		base.ActivePattern();
	}

	public override void ExitPattern()
	{
		ResetValues();
	}

	private void ResetValues()
	{
		AttackPoses = new List<int>(RepeatCount);
		attackCount = 0;
	}

	private IEnumerator ActiveShockWave()
	{
		yield return SetAttackOrder();

		for(int c = 0; c < RepeatCount; c++)
		{
			ShockWaveZones[AttackPoses[c]].gameObject.SetActive(true);
			yield return SetAttackOrder();
		}
	}

	private IEnumerator SetAttackOrder()
	{
		int whereIsAttack = -1;
		for (int c = 0; c < RepeatCount; c++)
		{
			whereIsAttack = Random.Range(0, 4);
			AttackPoses.Add(whereIsAttack);
			yield return new WaitForSeconds(0.3f);
		}
	}
}
