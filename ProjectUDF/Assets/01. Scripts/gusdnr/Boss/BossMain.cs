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
        Moving = 2,
        Attack = 3,
        Cooldown = 4,
        InCC = 5,
		Die = 6,
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
    public BossPattern[] ActivePatterns;

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
		foreach (var pattern in ActivePatterns)
		{
            pattern.Initialize(this);
		}
		foreach (var pattern in PassivePatterns)
		{
            pattern.Initialize(this);
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
            if(!IsAlive) break;
            passive?.ActivePattern();
			yield return PassiveWaits[count];
        }
    }

    private void SetState(BossState bs)
    {
        CurBossState = bs;
        switch (CurBossState)
        {
            case BossState.None:
                break;
            case BossState.Idle:
                break;
			case BossState.Moving:
				break;
			case BossState.Attack:
				SelectActivePattern();
                break;
			case BossState.Cooldown:
				break;
			case BossState.InCC:
				CancelAttack();
                break;
			case BossState.Die:
                DieBoss();
                break;
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

    private void SelectActivePattern()
    {
        if(SelectedPattern != null) SelectedPattern.ExitPattern();
        int selectIndex = UnityEngine.Random.Range(0,ActivePatterns.Length);
		SelectedPattern = ActivePatterns[selectIndex];
        SelectedPattern.EnterPattern();
    }

    private void StartCooldown()
    {
    
    }

    private void DieBoss()
    {
        if(CurBossState != BossState.Die) return;
        IsAlive = false;
		CancelAttack();
		StopPassive();
	}

    public void Damage(float damage)
    {
        CurHP = damage;
        if(CurHP <= 0)
        {
			SetState(BossState.Die);
		}
    }
}
