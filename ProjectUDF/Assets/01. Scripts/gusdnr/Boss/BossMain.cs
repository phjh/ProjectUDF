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
    [HideInInspector] public Transform TargetTrm;

    [Header("Manage Pattern")]
    [Range(0, 10)] public float PatternTerm;
    public BossPattern[] PassivePatterns;
    public BossPattern[] ActivePatterns;

    private List<Coroutine> PassiveCoroutines = new List<Coroutine>();
    private List<WaitForSeconds> PassiveWaits = new List<WaitForSeconds>();

	private void Awake()
	{
		IsAlive = true;
        CurHP = BossData.MaxHP;
		TargetTrm = GameManager.Instance.player.transform;
	}

	private void Start()
	{
	    foreach(var pattern in PassivePatterns)
        {
            StartPassivePattern(pattern);
        }	
	}

	public void StartPassivePattern(BossPattern passive)
    {
        float tickTime = passive.PassiveCool;
        PassiveWaits.Add(new WaitForSeconds(tickTime));
        PassiveCoroutines.Add(StartCoroutine(StartPassive(passive, PassiveWaits.Count - 1)));
    }

    private IEnumerator StartPassive(BossPattern passive, int count)
    {
        while (IsAlive)
        {
            passive?.ActivePattern();
			yield return PassiveWaits[count];
        }
    }
}
