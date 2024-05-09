using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreSlot : UIMono
{
	[Header("Slot Values")]
	[SerializeField] private bool IsMain;
	[Range(-1,1)][SerializeField] private int Index;
	public OreSO EquipOreType;

	private Image SlotImage;

	private void Awake()
	{
		SlotImage = GetComponentInChildren<Image>();
		if(EquipOreType == null) EquipDataInSlot((int)Stats.None);
		ShowUI();
	}

	public override void ShowUI()
	{
		int OreStatNumber = (int)Stats.None;
		if (IsMain)
		{
			OreStatNumber = (int)OreInventory.Instance.MainOreType;
		}
		else if (!IsMain)
		{
			OreStatNumber = (int)OreInventory.Instance.SubOreType[Index];
		}
		EquipDataInSlot(OreStatNumber);
	}

	public void UnEquipMainOre()
	{
		OreInventory.Instance.UnequipMain();
		EquipDataInSlot((int)Stats.None);
	}

	public void UnEquipSubOre()
	{
		OreInventory.Instance.UnequipSub(Index);
		EquipDataInSlot((int)Stats.None);
	}

	public void EquipDataInSlot(int dataIndex)
	{
		EquipOreType = UIManager.Instance.OreDatas[dataIndex];
		SlotImage.sprite = EquipOreType.OreSprite;
	}

	public override void CloseUI()
	{
		int OreStatNumber = (int)Stats.None;
		if (IsMain)
		{
			OreStatNumber = (int)OreInventory.Instance.MainOreType;
		}
		else if (!IsMain)
		{
			OreStatNumber = (int)OreInventory.Instance.SubOreType[Index];
		}
		EquipDataInSlot(OreStatNumber);
	}
}
