using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class MonoOre : MonoBehaviour
{
	[Header("UI Components")]
	public TMP_Text NameText;
	public TMP_Text DescText;

	[Header("SO Values")]
	public OreSO[] OreSOs;
	public OreSO CardOreSO;


	public void GetOre() => OreInventory.Instance.AddOre(CardOreSO.stats, CardOreSO.value);

	private void OnEnable()
	{
		SetRandomOre();
		Debug.Log($"Random Ore Setting Complete : {CardOreSO}");
	}

	#region �ܺ� ȣ��� �Լ���

	private int tempSO = 0;
	private void SetRandomOre()
	{
		tempSO = Random.Range(0, OreSOs.Length - 1);
		SetData();
	}

	public void SetHealOre()
	{
		tempSO = OreSOs.Length;
		SetData();
	}

	public void ResetOre()
	{
		int resetTempSO = Random.Range(0, OreSOs.Length - 1);
		while (resetTempSO == tempSO)
		{
			resetTempSO = Random.Range(0, OreSOs.Length - 1);
		}
		tempSO = resetTempSO;
		SetData();
	}

	public void SetData()
	{
		CardOreSO = OreSOs[tempSO];
		#region ���� ���ڿ� ���� �߰�
		string originalDesc = CardOreSO.OreDesc;
		string searchWord = "����";
		int index = originalDesc.IndexOf(searchWord);
		if (index == -1)
		{
			Debug.Log("�˻� ���ڿ��� ã�� �� �����ϴ�.");
			return;
		}
		// �˻� ���ڿ� �ٷ� �ڿ� �ٹٲ� ���ڿ��� �����Ͽ� ���ο� ���ڿ��� ����
		string desc = 
			originalDesc.Substring(0, index + searchWord.Length) + 
			Environment.NewLine	+ originalDesc.Substring(index + searchWord.Length);
		#endregion
		NameText.text = CardOreSO.OreName;
		DescText.text = desc;
	}

	#endregion
}
