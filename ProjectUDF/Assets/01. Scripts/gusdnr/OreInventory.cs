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
	public List<int> NormalOreList = new List<int>(4){ 0, 0, 0, 0 }; //�Ϲ� ���� ���� ����
    public List<int> UpgradeOreList = new List<int>(4){ 0, 0, 0, 0 }; //��ȭ ���� ���� ����
	[Range(0, 5)]public List<float> IncreaseValues = new List<float>(4); //���� ��ȭ�� �߰��� ���� �ɷ�ġ
	[Range(1, 5)]public int NeedToUpgrade = 3;
	private static int statNumber;

	#endregion

	private void Start()
	{
        ResetOreList();
	}
	
	#region Methods

	public void ResetOreList() //���� ��� �ʱ�ȭ��
	{
		NormalOreList = Enumerable.Repeat(0, 4).ToList();
		UpgradeOreList = Enumerable.Repeat(0, 4).ToList();
	}

	public void IncreaseOre(Stats statName, float statValue) //�ܺ� ȣ���� ���� ���� �Լ�
	{
		statNumber = (int)statName;
		NormalOreList[statNumber] += 1;

		status.EditStat(statName, statValue);
		CheckInventory(false, statNumber);
	}

	public void CheckInventory(bool isCheckAll = true, int index = 0) //bool���� false�� ���, index ������ Ȯ���ϴ� �κ��丮 ���� �Լ�
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
