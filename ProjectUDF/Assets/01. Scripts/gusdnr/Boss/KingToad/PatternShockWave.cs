using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_ShockWave", menuName = "SO/Boss/KingToad/Pattern_ShockWave")]
public class PatternShockWave : BossPattern
{
	[SerializeField] private int RepeatCount = 4;
	[SerializeField] private float AttackTerm = 0.5f;
	[SerializeField] private Color[] ZoneColors;


	private int attackCount = 0;
	private List<int> AttackPoses;
	private Collider2D[] ShockWaveZones;
	private Coroutine AttackCoroutine = null;

	public override void EnterPattern()
	{
		ResetValues();
		AttackCoroutine = bossMain.StartCoroutine(ActiveShockWave());
	}
	public override void ActivePattern()
	{
		if (AttackCoroutine == null)
		{
			bossMain.SetState(NextState[0]);
		}
	}

	public override void ExitPattern()
	{
		ResetValues();
	}

	private void ResetValues()
	{
		AttackPoses = new List<int>(RepeatCount);
		attackCount = 0;

		if (ShockWaveZones.Length <= 0)
		{
			Debug.Log($"Boss Object : [{bossMain.gameObject.name}]");
			Transform ZoneContainer = bossMain.gameObject.transform.Find("ZoneContainer");
			Debug.Log($"Find Zone Container : [{ZoneContainer != null}]");
			ShockWaveZones = ZoneContainer?.GetComponentsInChildren<Collider2D>();
		}
		for (int c = 0; c < ShockWaveZones.Length; c++)
		{
			if (ShockWaveZones[c].enabled && ShockWaveZones[c] != null) ShockWaveZones[c].enabled = false;
			SpriteRenderer render = ShockWaveZones[c].gameObject.GetComponent<SpriteRenderer>();
			render.color = ZoneColors[0];
		}
	}

	private IEnumerator ActiveShockWave()
	{
		yield return SetAttackOrder();
		for (attackCount = 0; attackCount < RepeatCount; attackCount++)
		{
			if (ShockWaveZones[AttackPoses[attackCount]] == null)
			{
				Debug.LogWarning($"Non Object [Index : {AttackPoses[attackCount]}]");
			}
			else
			{
				SpriteRenderer wavezoneRender = ShockWaveZones[AttackPoses[attackCount]].gameObject.GetComponent<SpriteRenderer>();
				wavezoneRender.color = ZoneColors[1];
				yield return wavezoneRender.DOColor(ZoneColors[2], AttackTerm);
				ShockWaveZones[AttackPoses[attackCount]].gameObject.SetActive(true);
				yield return new WaitForSeconds(AttackTerm);
				ShockWaveZones[AttackPoses[attackCount]].enabled = false;
				wavezoneRender.color = ZoneColors[0];
			}
		}

		IsActive = false;
		AttackCoroutine = null;
	}

	private IEnumerator SetAttackOrder()
	{
		int whereIsAttack = -1;
		for (int c = 0; c < RepeatCount; c++)
		{
			whereIsAttack = Random.Range(0, 3);
			AttackPoses.Add(whereIsAttack);
			yield return new WaitForSeconds(0.3f);
		}
	}
}
