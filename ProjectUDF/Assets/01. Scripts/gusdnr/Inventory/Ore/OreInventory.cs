using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OreInventory : MonoSingleton<OreInventory>
{
	#region Variables

	//Another Components
	private PlayerStats status;

	//Values
	public int MaxInInvnetory = 7; //광석 소지 개수
	public List<int> OreList = new List<int>(4) { 0, 0, 0, 0 }; //일반 광석 소지 개수
	public List<int> GemList = new List<int>(4) { 0, 0, 0, 0 }; //강화 광석 소지 개수
	[Range(1, 5)] public int NeedToUpgrade = 3; //업그레이드에 필요한 광석 개수

	private static int statNumber; //함수 사용시 스탯 번호를 가지고 있는 변수

	public Stats MainOreType;
	public List<Stats> SubOreType;

	public event Action ChangeContents;
	public event Action<Stats> OnChangeMainOre;
	#endregion

	private void Awake()
	{
		status = GameManager.Instance?.player.stat;
		if (status == null) Debug.LogError($"Player Status is NULL [Player : {GameManager.Instance.player}]");
		ResetOreList();
	}

	#region Methods

	public void ResetOreList() //광물 목록 초기화용
	{
		MainOreType = Stats.None;
		SubOreType = new List<Stats>(2){ Stats.None, Stats.None };

		OreList = Enumerable.Repeat(0, 4).ToList();
		GemList = Enumerable.Repeat(0, 4).ToList();

		ChangeContents?.Invoke();
	}

	#region Add Methods

	public void AddOre(Stats statName, int statValue) //외부 호출형 스탯 증가 함수
	{
		statNumber = (int)statName;
		if (statNumber == 4)
		{
			status.EditPlayerHP(statValue);
		}
		else
		{
			if (CheckCount() < MaxInInvnetory || OreList[statNumber] == 2)
			{
				OreList[statNumber] += 1;

				status.EditModifierStat(statName, statValue);
				CheckOreCount(false, statNumber);
			}
			else if (CheckCount() >= MaxInInvnetory && OreList[statNumber] != 2)
			{
				status.EditModifierStat(statName, 1);
			}
		}
	}

	private void AddGemStone(Stats statName)
	{
		statNumber = (int)statName;
		RemoveInventory(statNumber, 0, NeedToUpgrade);
		GemList[statNumber] += 1;
		status.EditModifierStat(statName, 5);
		status.EditModifierStat(statName, 2, true);
		CheckOreCount();
	}

	private void RemoveInventory(int statNumber, int statValue, int removeCount = 1)
	{
		UIManager.Instance.RemoveIcon(statNumber);
		OreList[statNumber] -= removeCount;
		if(statValue != 0) status.EditModifierStat((Stats)statNumber, statValue);
	}

	#endregion

	public void CheckOreCount(bool isCheckAll = true, int index = 0) // bool값이 false일 경우, index 값만을 확인하는 인벤토리 점검 함수
	{
		if (isCheckAll)
		{
			for (int i = 0; i < OreList.Count; i++)
			{
				if (OreList[i] >= NeedToUpgrade) AddGemStone((Stats)i);
			}
		}
		else
		{
			if (OreList[index] >= NeedToUpgrade)
			{
				AddGemStone((Stats)index);
			}
		}
	}

	private int CheckCount()
	{
		int NormalOreCount = OreList.Sum();
		int UpgradeOreCount = GemList.Sum();
		return NormalOreCount + UpgradeOreCount;
	}

	#region Equip Methods

	public void EquipMain(Stats statName)
	{
		if(MainOreType != Stats.None) MainOreType = Stats.None;
		statNumber = (int)statName;
		if (OreList[statNumber] > 0)
		{
			int value = UIManager.Instance.OreDatas[statNumber].value;
			RemoveInventory(statNumber, value);
		}
		else return;
		MainOreType = statName;

		UIManager.Instance._OreInfo.RefreshEquips();

		OnChangeMainOre?.Invoke(MainOreType);
		ChangeContents?.Invoke();
	}

	public void UnequipMain()
	{
		if(MainOreType != Stats.None) MainOreType = Stats.None;
		OnChangeMainOre?.Invoke(MainOreType);
		ChangeContents?.Invoke();
		UnequipSub(0);
		UnequipSub(1);
	}

	public void EquipSub(Stats statName, int index)
	{
		//if (index > SubOreType.Count) return;

		if (MainOreType != Stats.None)
		{
			if (SubOreType[index] != Stats.None)
			{
				UnequipSub(index);
			}

			statNumber = (int)statName;

			if (OreList[statNumber] > 0)
			{
				int value = UIManager.Instance.OreDatas[statNumber].value;
				RemoveInventory(statNumber, value);
			}
			else return;
			
			SubOreType[index] = statName;
		}
		else if (MainOreType == Stats.None)
		{
			return;
		}

		UIManager.Instance._OreInfo.RefreshEquips();

		ChangeContents?.Invoke();
	}

	public void UnequipSub(int index)
	{
		OreSO data = UIManager.Instance.OreDatas[(int)SubOreType[index]];
		if (data.stat != Stats.None)
		{
			AddOre(data.stat, data.value);
			UIManager.Instance.AddOreIcon(data);
		}
		SubOreType[index] = Stats.None;
		UIManager.Instance._OreInfo.RefreshEquips();

		ChangeContents?.Invoke();
	}
	
	#endregion

	#endregion
}
