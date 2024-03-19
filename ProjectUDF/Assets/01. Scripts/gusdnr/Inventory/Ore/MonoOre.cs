using UnityEngine;
using TMPro;
using System.Collections;

public class MonoOre : MonoBehaviour
{
	[Header("UI Components")]
	public TMP_Text NameText;
	public TMP_Text DescText;

	[Header("SO Values")]
	public OreSO[] OreDatas;
	public OreSO CardOreSO;

	public void GetOre() => OreInventory.Instance.AddOre(CardOreSO.stats, CardOreSO.value);

	private void OnEnable()
	{
		SetRandomOre();
		Debug.Log($"Random Ore Setting Complete : {CardOreSO}");
	}

	#region 외부 호출용 함수들
	private int tempSO = 0;
	private void SetRandomOre()
	{
		tempSO = Random.Range(0, OreDatas.Length);
		SetData();
	}

	public void ResetOre()
	{
		int resetTempSO = Random.Range(0, OreDatas.Length);
		while (resetTempSO == tempSO)
		{
			resetTempSO = Random.Range(0, OreDatas.Length);
		}
		tempSO = resetTempSO;
		SetData();
	}

	public void SetData()
	{
		CardOreSO = OreDatas[tempSO];
		#region 설명 문자열 단행 추가
		string desc = CardOreSO.OreDesc;
		desc = desc.Replace(",", "\n");
		#endregion
		NameText.text = CardOreSO.OreName;
		DescText.text = desc;
	}

	#endregion
}
