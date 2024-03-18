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
public class PlayerStat : ScriptableObject
{
	public event Action<float> StrengthChanged;
	public event Action<float> HpChanged;
	public event Action<float> MoveSpeedChanged;
	public event Action<float> AttackSpeedChanged;
	public event Action<float> LuckyChanged;

	[Header("Player's Stats")]
	public int MaxHP;
	private int curHP;
	public int CurHP { get; set; }
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

		Type characterStatType = typeof(PlayerStat);
		foreach (Stats statType in Enum.GetValues(typeof(Stats)))
		{
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
	}

	public PlayerStat Clone() //Player 스탯을 복제 후, 돌려준다.
	{
		var returnvalue = Instantiate(this);
		return returnvalue;
	}

	protected Player _owner;
	public void SetOwner(Player owner)
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
		curHP = MaxHP;
	}

	public void EditPlayerHP(int value)
	{
		curHP += value;
		if (curHP > MaxHP) { curHP = MaxHP; }
		if(curHP <= 0) {  } //플레이어 쪽에서 죽는 이벤트 실행
	}

	#endregion
}
