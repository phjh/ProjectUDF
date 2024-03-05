using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Ore Inventory List", menuName = "SO/Player/OreInventory")]
public class OreInventory : MonoSingleton<OreInventory>
{
	#region Variables
	
	//Another Components
	public PlayerStat status;

	//Values
	public List<int> NormalOreList = new List<int>(4){ 0, 0, 0, 0 };
    public List<int> UpgradeOreList = new List<int>(4){ 0, 0, 0, 0 };
	[Range(1, 5)]public List<float> IncreaseValues = new List<float>(5);
	[Range(1, 5)]public int NeedToUpgrade = 3;
	private static int statNumber;

	#endregion

	private void Start()
	{
        ResetOreList();
		status = GetComponent<PlayerStat>();
	}
	
	#region Methods

	public void ResetOreList() //���� ��� �ʱ�ȭ��
	{
		NormalOreList.ForEach(o => { NormalOreList.Insert(o, 0); Debug.Log($"Normal {o} : 0"); });
		UpgradeOreList.ForEach(o => { UpgradeOreList.Insert(o, 0); Debug.Log($"Upgrade {o} : 0"); });
	}

	public void IncreaseOre(Stats statName, float statValue) //�ܺ� ȣ���� ���� ���� �Լ�
	{
		statNumber = (int)statName;
		NormalOreList[statNumber] += 1;

		status.EditStat(statName, statValue);
	}

	public void CheckInventory(bool isCheckAll = true, int index = 0) //bool���� false�� ���, index ������ Ȯ���ϴ� �κ��丮 ���� �Լ�
	{
		if (isCheckAll)
		{
			foreach (int count in NormalOreList)
			{
				index++;
				Debug.Log($"{index}�� ���� : {count} ��");
				if (count < NeedToUpgrade) continue;
				if (count == NeedToUpgrade)
				{
					UpgradeOre((Stats)index, IncreaseValues[index]);
				}
			}
		}
		else
		{
			if (NormalOreList[index] == NeedToUpgrade)
			{
				UpgradeOre((Stats)index, IncreaseValues[index]);
			}
		}
	}

	private void UpgradeOre(Stats statName, float statValue)
	{
		statNumber = (int)(statName);
		NormalOreList[statNumber] = 0;
		status.EditStat(statName, statValue);
	}

	#endregion
}
