using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class MonoOre : MonoBehaviour
{
	public static event Action FailToResearch;

	[Header("UI Components")]
	public TMP_Text NameText;
	public TMP_Text DescText;
	public Image OreImage;
	public Button LinkedBtn;

	[Header("SO Values")]
	public OreSO[] OreDatas;
	public OreSO CurOreData;

	public void GetOre() => OreInventory.Instance.AddOre(CurOreData.stats, CurOreData.value);

	private void OnEnable()
	{
		SetRandomOre();
		Debug.Log($"Random Ore Setting Complete : {CurOreData}");
	}

	#region 외부 호출용 함수들
	private int tempSO = 0;
	private void SetRandomOre()
	{
		tempSO = Random.Range(0, OreDatas.Length);
		LinkedBtn.interactable = true;
		OreImage.color = new Vector4(1, 1, 1, 1);
		if (FailToResearch == null) FailToResearch += UIManager.Instance.CountFail;
		SetData();
	}

	public void ResetOre()
	{
		float successRate = Random.value;
		if( successRate <= 0.3f) //추후 재채광 성공 확률로 치환 예정
		{
			int resetTempSO = Random.Range(0, OreDatas.Length);
			while (resetTempSO == tempSO)
			{
				resetTempSO = Random.Range(0, OreDatas.Length);
			}
			tempSO = resetTempSO;
			SetData();
		}
		else
		{
			FailToResearch?.Invoke();
			OreImage.color = new Vector4(1, 1, 1, 0f);
			NameText.text = string.Empty;
			DescText.text = string.Empty;
			LinkedBtn.interactable = false;
		}
	}

	public void SetData()
	{
		CurOreData = OreDatas[tempSO];
		#region 설명 문자열 단행 추가
		string desc = CurOreData.OreDesc;
		desc = desc.Replace(",", "\n");
		#endregion
		NameText.text = CurOreData.OreName;
		DescText.text = desc;
	}

	#endregion
}
