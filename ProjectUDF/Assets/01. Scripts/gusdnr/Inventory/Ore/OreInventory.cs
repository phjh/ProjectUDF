using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu(fileName = "Ore Inventory List", menuName = "SO/Player/OreInventory")]
public class OreInventory : MonoSingleton<OreInventory>
{
	#region Variables

	//Another Components
	public PlayerStat status; //추후 플레이어가 가지고 있는 플레이어 스탯 SO 가져오는 부분 추가 필요 *****

	//Values
	public int MaxInInvnetory = 7; //광석 소지 개수
	public List<int> OreList = new List<int>(4) { 0, 0, 0, 0 }; //일반 광석 소지 개수
	public List<int> GemStoneList = new List<int>(4) { 0, 0, 0, 0 }; //강화 광석 소지 개수
	[Range(1, 5)] public int NeedToUpgrade = 3; //업그레이드에 필요한 광석 개수

	private static int statNumber; //함수 사용시 스탯 번호를 가지고 있는 변수
	#endregion

	private void Start()
	{
		ResetOreList();
	}

	#region Methods

	public void ResetOreList() //광물 목록 초기화용
	{
		OreList = Enumerable.Repeat(0, 4).ToList();
		GemStoneList = Enumerable.Repeat(0, 4).ToList();
	}

	#region Add Methods

	public void AddOre(Stats statName, int statValue) //외부 호출형 스탯 증가 함수
	{
		int statNumber = (int)statName;
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
		OreList[statNumber] -= NeedToUpgrade;
		GemStoneList[statNumber] += 1;
		status.EditModifierStat(statName, 5);
		status.EditModifierStat(statName, 2, true);
		CheckOreCount();
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
		int UpgradeOreCount = GemStoneList.Sum();
		return NormalOreCount + UpgradeOreCount;
	}

	#endregion
}
