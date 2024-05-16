using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossMain : MonoBehaviour
{
    public enum BossState
    {
        None = 0,
        Idle = 1,
        Cooldown = 2,
        Attack = 3,
        InCC = 4,
		Died = 5,
    }

    public struct PatternPair
    {
        [Range(0,100)] public float SelectPersent;
        public BossPattern DoPattern;
    }

    [Header("Boss Status")]
    public BossDataSO BossData;
    public BossState CurBossState;
    public float CurHP;
    public bool IsAlive;
    [HideInInspector] public Transform TargetTrm;

    [Header("Manage Pattern")]
    [Range(0, 10)] public float PatternTerm;
    public BossPattern MovingPattern;
    public BossPattern CooldownPattern;
    public BossPattern[] PassivePatterns;
    public PatternPair[] ActivePatterns;

    private Animator ConditionSetter;
    private List<Coroutine> PassiveCoroutines = new List<Coroutine>();
    private List<WaitForSeconds> PassiveWaits = new List<WaitForSeconds>();
    private BossPattern SelectedPattern;

    private bool IsAttack { get; set; } = false;
    private bool IsOutCC { get; set; } = false;
    private bool IsCooldown { get; set; } = false;
    private bool IsMoving { get; set; } = false;

	private void Awake()
	{
		IsAlive = true;
        CurHP = BossData.MaxHP;
		ConditionSetter = GetComponent<Animator>();
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

    private void SetState(BossState bs)
    {
        CurBossState = bs;
        if(CurBossState == BossState.InCC)
        {
            CancelAttack();
        }
        if(CurBossState == BossState.Died)
        {
            CancelAttack();
			StopPassive();
        }
    }

	private void StopPassive()
	{
		for(int c = 0; c <  PassiveWaits.Count; c++)
        {
            StopCoroutine(PassiveCoroutines[c]);
        }
	}

	private void CancelAttack()
	{
		if(SelectedPattern != null)
        {
            SelectedPattern.ExitPattern();
        }
	}

    private void SelectPattern()
    {
        int SelectedPattern; 
        //ActivePatterns[]
    }
}
