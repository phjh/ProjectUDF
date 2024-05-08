using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreSlot : UIMono
{
	[SerializeField] private bool IsMain;
	[Range(0,1)][SerializeField] private int Index;
	public OreSO EquipOreType;
	private Image SlotImage;

	private void Awake()
	{
		SlotImage = GetComponent<Image>();
	}

	public override void ShowUI()
	{
		if (IsMain)
		{
			int OreStatNumber = (int)OreInventory.Instance.MainOreType;
			EquipOreType = UIManager.Instance.OreDatas[OreStatNumber];
		}
		else if (!IsMain)
		{
			int OreStatNumber = (int)OreInventory.Instance.SubOreType[Index];
			EquipOreType = UIManager.Instance.OreDatas[OreStatNumber];
		}
		EquipOre();
	}

	public void EquipOre()
	{
		SlotImage.sprite = EquipOreType.OreSprite;
	}

	public void UnEquipMainOre()
	{
		OreInventory.Instance.UnequipMain();
		EquipOreType = UIManager.Instance.OreDatas[(int)Stats.None];
		SlotImage.sprite = EquipOreType.OreSprite;
	}

	public void UnEquipSubOre()
	{
		OreInventory.Instance.UnequipSub(Index);
		EquipOreType = UIManager.Instance.OreDatas[(int)Stats.None];
		SlotImage.sprite = EquipOreType.OreSprite;
	}

	public override void CloseUI()
	{
		if(EquipOreType != null)
		{
			return;
		}
	}
}
