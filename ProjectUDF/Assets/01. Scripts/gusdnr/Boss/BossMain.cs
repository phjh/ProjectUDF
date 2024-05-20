using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public BossPattern IdlePattern;
    public BossPattern MovingPattern;
    public BossPattern CooldownPattern;
    public BossPattern InCCPattern;
    public BossPattern[] PassivePatterns;
    public BossPattern[] ActivePatterns;

    
    private List<Coroutine> PassiveCoroutines = new List<Coroutine>();
    private List<WaitForSeconds> PassiveWaits = new List<WaitForSeconds>();
    private BossStateMachine StateMachine;

    private bool CanMove { get; set ; } = true;
    private bool IsAttack { get; set; } = false;
    private bool IsOutCC { get; set; } = false;
    private bool IsCooldown { get; set; } = false;
    private bool IsMoving { get; set; } = false;

	private void Awake()
	{
		IsAlive = true;
        CurHP = BossData.MaxHP;
		TargetTrm = GameManager.Instance.player.transform;
	}

	private void Start()
	{
		IdlePattern.Initialize(this);
		MovingPattern.Initialize(this);
		CooldownPattern.Initialize(this);               
		InCCPattern.Initialize(this);

		foreach (var pattern in ActivePatterns)
		{
            pattern.Initialize(this);
		}
		for(int p = 0; p < PassivePatterns.Length; p++)
		{
            PassivePatterns[p].Initialize(this);
			StartPassivePattern(PassivePatterns[p], p);
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
				StateMachine.ChangeState(IdlePattern);
				break;
			case BossState.Moving:
                if(!CanMove) return;
				StateMachine.ChangeState(MovingPattern);
				break;
			case BossState.Attack:
				SelectActivePattern();
				break;
			case BossState.Cooldown:
				StartCooldown();
				break;
			case BossState.InCC:
				StateMachine.ChangeState(InCCPattern);
				break;
			case BossState.Die:
				DieBoss();
				break;
		}
	}

	#region Passive Methods

	public void StartPassivePattern(BossPattern passive, int count)
    {
        float tickTime = passive.PassiveCool;
        PassiveWaits.Add(new WaitForSeconds(tickTime));
        PassiveCoroutines.Add(StartCoroutine(ActivePassive(passive, count)));
    }

    private IEnumerator ActivePassive(BossPattern passive, int count)
    {
        while (IsAlive)
        {
            if(!IsAlive) break;
            passive?.ActivePattern();
			yield return PassiveWaits[count];
        }
    }

	private void StopPassive()
	{
		for(int c = 0; c <  PassiveWaits.Count; c++)
        {
            StopCoroutine(PassiveCoroutines[c]);
        }
	}

	#endregion

    private void SelectActivePattern()
    {
        int selectIndex = UnityEngine.Random.Range(0,ActivePatterns.Length);
        StateMachine.ChangeState(ActivePatterns[selectIndex]);
    }

    private void StartCooldown()
    {
        IsCooldown = true;
		StateMachine.ChangeState(CooldownPattern);
	}

    private void DieBoss()
    {
        if(CurBossState != BossState.Die) return;
        IsAlive = false;
		StateMachine.CancelAttack();
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
