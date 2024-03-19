using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonoOre : MonoBehaviour
{
	[Header("UI Components")]
	public TMP_Text NameText;
	public TMP_Text DescText;
	public Image OreImage;
	public Button LinkedBtn;

	[Header("SO Values")]
	public OreSO[] OreDatas;
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
		tempSO = Random.Range(0, OreDatas.Length);
		SetData();
	}

	public void ResetOre()
	{
		float successRate = Random.value;
		if( successRate <= 0.3f) //���� ��ä�� ���� Ȯ���� ġȯ ����
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
			OreImage.color = new Vector4(1, 1, 1, 0);
			NameText.text = string.Empty;
			DescText.text = "�� ä���� �����ع��ȴ�...";
			LinkedBtn.interactable = false;
		}
	}

	public void SetData()
	{
		CardOreSO = OreDatas[tempSO];
		#region ���� ���ڿ� ���� �߰�
		string desc = CardOreSO.OreDesc;
		desc = desc.Replace(",", "\n");
		#endregion
		NameText.text = CardOreSO.OreName;
		DescText.text = desc;
	}

	#endregion
}
