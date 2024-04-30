using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum Stats
{
    Strength = 0,
    Lucky = 1,
    MoveSpeed = 2,
    AttackSpeed = 3,
    HP = 4
}

[CreateAssetMenu(fileName = "New Player Stat", menuName = "SO/Player/PlayerStat")]
public class PlayerStats : ScriptableObject
{
	public static event Action HpChanged;
	public static event Action OnDeadPlayer;

	public static Action OnStatChanged;

	[Header("Player's Stats")]
	public int MaxHP;
	public int curHP;
	public int CurHP
	{ get { return curHP;} set{ curHP = value; } }
	public Stat Strength;
	public Stat MoveSpeed;
	public Stat AttackSpeed;
	public Stat Lucky;

	protected Dictionary<Stats, FieldInfo> _fieldInfoDictionary;

	public void OnEnable()
	{
		Debug.Log("컴파일 시 초기화 실행");

		if (_fieldInfoDictionary == null)
		{
			_fieldInfoDictionary = new Dictionary<Stats, FieldInfo>();
		}
		_fieldInfoDictionary.Clear();

		Type characterStatType = typeof(PlayerStats);
		foreach (Stats statType in Enum.GetValues(typeof(Stats)))
		{
			if(statType == Stats.HP) break;
			FieldInfo statField = characterStatType.GetField(statType.ToString());
			if (statField == null)
			{
				Debug.LogError($"There are no stat! error : {statType.ToString()}");
			}
			else
			{
				_fieldInfoDictionary.Add(statType, statField);
			}
		}

		if(TimeManager.Instance != null)TimeManager.Instance.OnTimerEnd += DiePlayer;
	}

	public PlayerStats Clone() //Player 스탯을 복제 후, 돌려준다.
	{
		var returnvalue = Instantiate(this);
		returnvalue._owner = _owner;
		return returnvalue;
	}

	protected PlayerMain _owner;
	public void SetOwner(PlayerMain owner)
	{
		_owner = owner;
		ResetHP();
	}

	public Stat GetStatByType(Stats type)
	{
		return _fieldInfoDictionary[type].GetValue(this) as Stat;
	}

	public void IncreaseStatBy(int modifyValue, float duration, Stats statType) //버프형 값 적용
	{
		_owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statType));

    }

	protected IEnumerator StatModifyCoroutine(int modifyValue, float duration, Stats statType, bool IsFixed = false) //버프 코루틴
	{
		Stat target = GetStatByType(statType);
		target.AddModifier(modifyValue, IsFixed);
		yield return new WaitForSeconds(duration);
		target.RemoveModifier(modifyValue, IsFixed);
	}

	#region 플레이어 능력치 변경 함수

	public void EditModifierStat(Stats statType, float value, bool isPersent = false)
	{
		GetStatByType(statType).AddModifier(value, isPersent);

    }

	public void ResetHP()
	{
		CurHP = MaxHP;
	}

	public void EditPlayerHP(int value)
	{
		curHP += value;
		if (curHP > MaxHP) { curHP = MaxHP; }
		else { HpChanged?.Invoke(); }

		if(curHP <= 0) { DiePlayer(); } //플레이어 쪽에서 죽는 이벤트 실행
	}

	public void DiePlayer()
	{
		OnDeadPlayer?.Invoke();
	}

	#endregion
}
