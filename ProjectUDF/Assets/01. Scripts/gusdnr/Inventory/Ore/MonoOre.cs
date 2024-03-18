using UnityEngine;
using TMPro;

public class MonoOre : MonoBehaviour
{
	[Header("UI Components")]
	public TMP_Text NameText;
	public TMP_Text DescText;

	[Header("SO Values")]
	public OreSO[] OreSOs;
	public OreSO CardOreSO;

	private int tempSO = 0;

	public void GetOre() => OreInventory.Instance.AddOre(CardOreSO.stats, CardOreSO.value);

	private void OnEnable()
	{
		SetRandomOre();
		Debug.Log($"Random Ore Setting Complete : {CardOreSO}"); 
	}

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

		NameText.text = CardOreSO.name;
		DescText.text = CardOreSO.desc;
	}
}
