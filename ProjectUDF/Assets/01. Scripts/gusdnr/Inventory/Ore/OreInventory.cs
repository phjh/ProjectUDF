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
	public PlayerStat status; //���� �÷��̾ ������ �ִ� �÷��̾� ���� SO �������� �κ� �߰� �ʿ� *****
	
	//Values
	public int MaxInInvnetory = 7; //���� ���� ����
	public List<int> OreList = new List<int>(4){ 0, 0, 0, 0 }; //�Ϲ� ���� ���� ����
    public List<int> GemStoneList = new List<int>(4){ 0, 0, 0, 0 }; //��ȭ ���� ���� ����
	[Range(0, 5)]public List<float> IncreaseValues = new List<float>(4); //���� ��ȭ�� �߰��� ���� �ɷ�ġ
	[Range(1, 5)]public int NeedToUpgrade = 3; //���׷��̵忡 �ʿ��� ���� ����

	private static int statNumber; //�Լ� ���� ���� ��ȣ�� ������ �ִ� ����
	#endregion

	private void Start()
	{
        ResetOreList();
	}

	#region Methods

	public void ResetOreList() //���� ��� �ʱ�ȭ��
	{
		OreList = Enumerable.Repeat(0, 4).ToList();
		GemStoneList = Enumerable.Repeat(0, 4).ToList();
	}

	#region Add Methods

	public void AddOre(Stats statName, float statValue) //�ܺ� ȣ���� ���� ���� �Լ�
	{
		int statNumber = (int)statName;
		if (CheckCount() < MaxInInvnetory || OreList[statNumber] == 2)
		{
			OreList[statNumber] += 1;

			status.EditModifierStatToFixed(statName, statValue);
			CheckOreCount(false, statNumber);
		}
		else if(CheckCount() >= MaxInInvnetory && OreList[statNumber] != 2)
		{
			status.EditModifierStatToFixed(statName, 1);
		}
	}

	private void AddGemStone(Stats statName)
	{
		statNumber = (int)statName;
		OreList[statNumber] -= NeedToUpgrade;
		GemStoneList[statNumber] += 1;
		status.EditModifierStatToFixed(statName, 5);
		status.EditModifierStatToPersent(statName, 2);
		CheckOreCount();
	}

	#endregion

	public void CheckOreCount(bool isCheckAll = true, int index = 0) // bool���� false�� ���, index ������ Ȯ���ϴ� �κ��丮 ���� �Լ�
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
