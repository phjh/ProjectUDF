using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : MonoBehaviour
{
    public enum BossState
    {
        None = 0,
        Cooldown = 1,
        Attack = 2,
        InCC = 3,
		Died = 4,
    }

    [Header("Boss Status")]
    public BossDataSO BossData;
    public BossState CurBossState;
    public float CurHP;
    public bool IsAlive;

    [Header("Manage Pattern")]
    [Range(0, 10)] public float PatternTerm;
    public BossPassive[] PassivePatterns;
    public BossPattern[] ActivePatterns;

    private List<Coroutine> PassiveCoroutines = new List<Coroutine>();

	private void Awake()
	{
		IsAlive = true;
        CurHP = BossData.MaxHP;
	}

	private void Start()
	{
	    foreach(var pattern in PassivePatterns)
        {
            StartPassivePattern(pattern);
        }	
	}

	public void StartPassivePattern(BossPassive passive)
    {
        float tickTime = passive.ActiveTickTime;
        PassiveCoroutines.Add(StartCoroutine(StartPassive(tickTime, passive)));
    }

    private IEnumerator StartPassive(float tick, BossPassive passive)
    {
        while (IsAlive)
        {
            passive?.PassiveActive();
			yield return new WaitForSeconds(tick);
        }
    }
}