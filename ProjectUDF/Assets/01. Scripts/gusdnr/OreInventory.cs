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
	public PlayerStat status;

	//Values
	public List<int> NormalOreList = new List<int>(4){ 0, 0, 0, 0 }; //일반 광석 소지 개수
    public List<int> UpgradeOreList = new List<int>(4){ 0, 0, 0, 0 }; //강화 광석 소지 개수
	[Range(0, 5)]public List<float> IncreaseValues = new List<float>(4); //광석 강화시 추가로 얻을 능력치
	[Range(1, 5)]public int NeedToUpgrade = 3;
	private static int statNumber;

	#endregion

	private void Start()
	{
        ResetOreList();
	}
	
	#region Methods

	public void ResetOreList() //광물 목록 초기화용
	{
		NormalOreList = Enumerable.Repeat(0, 4).ToList();
		UpgradeOreList = Enumerable.Repeat(0, 4).ToList();
	}

	public void IncreaseOre(Stats statName, float statValue) //외부 호출형 스탯 증가 함수
	{
		statNumber = (int)statName;
		NormalOreList[statNumber] += 1;

		status.EditStat(statName, statValue);
		CheckInventory(false, statNumber);
	}

	public void CheckInventory(bool isCheckAll = true, int index = 0) //bool값이 false일 경우, index 값만을 확인하는 인벤토리 점검 함수
	{
		if (isCheckAll)
		{
			foreach (int count in NormalOreList)
			{
				index++;
				if (count < NeedToUpgrade) continue;
				UpgradeOre((Stats)index, IncreaseValues[index]);
			}
		}
		else
		{
			if (NormalOreList[index] >= NeedToUpgrade)
			{
				UpgradeOre((Stats)index, IncreaseValues[index]);
			}
		}
	}

	private void UpgradeOre(Stats statName, float statValue)
	{
		statNumber = (int)(statName);
		NormalOreList[statNumber] -= NeedToUpgrade;
		UpgradeOreList[statNumber] += 1;
		status.EditStat(statName, statValue);
		CheckInventory();
	}

	#endregion
}
