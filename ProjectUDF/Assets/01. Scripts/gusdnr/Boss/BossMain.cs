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
    public bool IsAlive;
    public bool CanMove;
    [HideInInspector] public Transform TargetTrm;

    [Header("Manage Pattern")]
    [Range(0, 10)] public float PatternTerm;
    [Header("Insert Pattern")]
    public BossPattern IdlePattern;
    public BossPattern MovingPattern;
    public BossPattern CooldownPattern;
    public BossPattern InCCPattern;
    public BossPattern[] PassivePatterns;
    public BossPattern[] ActivePatterns;

    
    private List<Coroutine> PassiveCoroutines = new List<Coroutine>();
    private List<WaitForSeconds> PassiveWaits = new List<WaitForSeconds>();
    private Rigidbody2D BossRB;
    public BossStateMachine StateMachine { get; set; }

    public float CurHP { get; set; }
    public float MaxHP{ get; set; }

    public bool IsAttack { get; set; } = false;
    public bool IsHaveCC { get; set; } = false;
    public bool IsCooldown { get; set; } = false;
	public bool IsMoving { get; set; } = false;

	private void Awake()
	{
		IsAlive = true;
        CurHP = MaxHP = BossData.MaxHP;
		BossRB = GetComponent<Rigidbody2D>();
		TargetTrm = GameManager.Instance.player.transform;
	}

	private void Start()
	{
		if (StateMachine == null) StateMachine = new BossStateMachine();

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
            Debug.Log("Active Passive");
		}

        StateMachine.Initialize(IdlePattern);
	}
    
	private void Update()
	{
		StateMachine.CurrentPattern?.ActivePattern();
	}

	public void SetState(BossState bs)
	{
        Debug.Log($"Before State : [{CurBossState}]");
		CurBossState = bs;
		switch (CurBossState)
		{
			case BossState.None:
				break;
			case BossState.Idle:
				StateMachine.ChangeState(IdlePattern);
				break;
			case BossState.Moving:
                if(!CanMove) SetState(BossState.Idle);
				else StateMachine.ChangeState(MovingPattern);
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
        Debug.Log($"Before State : [{CurBossState}]");
	}

	#region Passive Methods

	public void StartPassivePattern(BossPattern passive, int count)
    {
        float tickTime = passive.IfPassiveCool;
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
		Invoke(nameof(ChangeStateOutTime), PatternTerm);
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

    public void GetCC(float ccTIme)
    {
		IsHaveCC = true;
        SetState(BossState.InCC);
        Invoke(nameof(ChangeStateOutTime),ccTIme);
	}

    private void ChangeStateOutTime(BossState NextState = BossState.Idle)
    {
        SetState(NextState);
    }
}
