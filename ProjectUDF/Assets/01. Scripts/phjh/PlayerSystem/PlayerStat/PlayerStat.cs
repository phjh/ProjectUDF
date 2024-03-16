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
    public Stat Strength;
    public Stat HP;
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
	}

	public Stat GetStatByType(Stats type)
	{
		return _fieldInfoDictionary[type].GetValue(this) as Stat;
	}

	public void IncreaseStatBy(int modifyValue, float duration, Stats statType) //버프형 값 적용
	{
		_owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statType));
	}

	protected IEnumerator StatModifyCoroutine(int modifyValue, float duration, Stats statType, bool IsFixed = false)
	{
		Stat target = GetStatByType(statType);
		target.AddModifier(modifyValue, IsFixed);
		yield return new WaitForSeconds(duration);
		target.RemoveModifier(modifyValue, IsFixed);
	}

	#region 플레이어 능력치 증가 구분

	public void ModifyStatToPersent(Stats statType, float value)
	{
		GetStatByType(statType).AddModifier(value);
	}

	public void ModifyStatToFixedValue(Stats statType, float value)
	{
		GetStatByType(statType).AddModifier(value);
	}

	#endregion

	
	//기본적으로 추가스텟이니 유의할것
	public void EditStat(Stats statName, float EditingAmount)
	{
		switch (statName)
		{
			case Stats.Strength:
				Strength.SetRealValue(EditingAmount);
				StrengthChanged?.Invoke(Strength.realValue);
				break;
			case Stats.HP:
				HP.SetRealValue(EditingAmount);
				HpChanged?.Invoke(HP.realValue);
				break;
			case Stats.MoveSpeed:
				MoveSpeed.SetRealValue(EditingAmount);
				MoveSpeedChanged?.Invoke(MoveSpeed.realValue);
				break;
			case Stats.AttackSpeed:
				AttackSpeed.SetRealValue(EditingAmount);
				AttackSpeedChanged?.Invoke(AttackSpeed.realValue);
				break;
			case Stats.Lucky:
				Lucky.SetRealValue(EditingAmount);
				LuckyChanged?.Invoke(Lucky.realValue);
				break;
		}
	}
}
