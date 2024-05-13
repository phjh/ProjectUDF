using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class OreCard : UIMono
{
	[HideInInspector] public static event Action FailToResearch;

	[Header("Ore Card UI Values")]
	public bool isSelect = false;
	protected bool isActive;

	[Header("UI Components")]
	public TMP_Text NameText;
	public TMP_Text DescText;
	public Image OreImage;
	public Button LinkedBtn;
	[SerializeField] private GameObject ResetBtn;

	public OreSO CurOreData;

	private void Awake()
	{
		SetRandomOre();
		DOTween.Init();
		transform.localScale = Vector3.one * 0.1f;
		UIManager.Instance.Cards.Add(this);
		ResetBtn.SetActive(false);
		gameObject.SetActive(false);
		isActive = false;
	}

	#region Management UI Visiable

	public override void ShowUI()
	{
		SetRandomOre();
		gameObject.SetActive(true);
		isActive = true;

		// DOTween 함수를 차례대로 수행하게 해줍니다.
		var seq = DOTween.Sequence();

		// DOScale 의 첫 번째 파라미터는 목표 Scale 값, 두 번째는 시간입니다.
		seq.Append(transform.DOScale(1.3f, 0.3f));
		seq.Append(transform.DOScale(1f, 0.2f));

		seq.Play().OnComplete(() => {
			ResetBtn.SetActive(true);
		});
	}

	public void CloseDefault()
	{
		var seq = DOTween.Sequence();

		transform.localScale = Vector3.one * 0.1f;

		seq.Append(transform.DOScale(1f, 0.1f));
		seq.Append(transform.DOScale(0.2f, 0.3f));


		seq.Play().OnComplete(() =>
		{
			isActive = false;
			gameObject.SetActive(false);
			ResetBtn.SetActive(false);
			UIManager.Instance.CloseMining();
		});
	}
	
	public override void CloseUI()
	{
		var seq = DOTween.Sequence();
		GetOre();
		transform.localScale = Vector3.one * 0.1f;

		seq.Append(transform.DOScale(1.1f, 0.1f));
		seq.Append(transform.DOScale(0.2f, 0.3f));

		seq.Play().OnComplete(() =>
		{
			isActive = false;
			if (UIManager.Instance.Cards != null) UIManager.Instance.Cards.ForEach(x =>
			{
				if (x.isActive) x.CloseDefault();
			});
			gameObject.SetActive(false);
			ResetBtn.SetActive(false);
		});
	}

	#endregion

	#region Manage Ores

	public void GetOre() => OreInventory.Instance.AddOre(CurOreData.stat, CurOreData.value);

	private int tempSO = 0;
	private void SetRandomOre()
	{
		tempSO = Random.Range(0, (int)Stats.HP);
		LinkedBtn.interactable = true;
		OreImage.color = new Vector4(1, 1, 1, 1);
		if (FailToResearch == null) FailToResearch += UIManager.Instance.CountFail;
		SetData();
	}

	public void ResetOre()
	{
		float successRate = Random.value;
		if (successRate <= 0.8f) //추후 재채광 성공 확률로 치환 예정
		{
			int resetTempSO = Random.Range(0, (int)Stats.HP);
			while (resetTempSO == tempSO)
			{
				resetTempSO = Random.Range(0, (int)Stats.HP);
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
		CurOreData = UIManager.Instance.OreDatas[tempSO];
		OreImage.sprite = CurOreData.OreSprite;
		#region 설명 문자열 단행 추가
		string desc = CurOreData.OreDesc;
		desc = desc.Replace(",", "\n");
		#endregion
		NameText.text = CurOreData.OreName;
		DescText.text = desc;
	}

	#endregion

}
