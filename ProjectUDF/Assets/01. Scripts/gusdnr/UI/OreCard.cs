using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OreCard : UIMono
{
	protected bool isActive;

	[Header("UI Components")]
	public TMP_Text NameText;
	public TMP_Text DescText;
	public Image OreImage;
	public Button LinkedBtn;
	[SerializeField] private GameObject ResetBtn;
	
	private MiningOre MiningData;

	private void Awake()
	{
		DOTween.Init();
		transform.localScale = Vector3.one * 0.1f;
		MiningData = GetComponentInChildren<MiningOre>();
		UIManager.Instance.Cards.Add(this);
		ResetBtn.SetActive(false);
		gameObject.SetActive(false);
		isActive = false;
	}

	public void Show()
	{
		gameObject.SetActive(true);
		isActive = true;

		// DOTween �Լ��� ���ʴ�� �����ϰ� ���ݴϴ�.
		var seq = DOTween.Sequence();

		// DOScale �� ù ��° �Ķ���ʹ� ��ǥ Scale ��, �� ��°�� �ð��Դϴ�.
		seq.Append(transform.DOScale(1.3f, 0.3f));
		seq.Append(transform.DOScale(1f, 0.2f));
		
		seq.Play().OnComplete(() => {
			ResetBtn.SetActive(true);
		});
	}

	public void HideDefault()
	{
		var seq = DOTween.Sequence();

		transform.localScale = Vector3.one * 0.1f;

		seq.Append(transform.DOScale(1f, 0.1f));
		seq.Append(transform.DOScale(0.2f, 0.2f));


		seq.Play().OnComplete(() =>
		{
			isActive = false;
			gameObject.SetActive(false);
			ResetBtn.SetActive(false);
			UIManager.Instance.CloseMining();
		});
	}

	public void HideSelect()
	{
		var seq = DOTween.Sequence();

		transform.localScale = Vector3.one * 0.1f;

		seq.Append(transform.DOScale(1.1f, 0.1f));
		seq.Append(transform.DOScale(0.2f, 0.3f));


		// OnComplete �� seq �� ������ �ִϸ��̼��� �÷��̰� �Ϸ�Ǹ�
		// { } �ȿ� �ִ� �ڵ尡 ����ȴٴ� �ǹ��Դϴ�.
		// ���⼭�� �ݱ� �ִϸ��̼��� �Ϸ�� �� ��ü�� ��Ȱ��ȭ �մϴ�.
		seq.Play().OnComplete(() =>
		{
			isActive = false;
			if(UIManager.Instance.Cards != null) UIManager.Instance.Cards.ForEach(x =>
			{
				if(x.isActive) x.HideDefault();
			});
			gameObject.SetActive(false);
			ResetBtn.SetActive(false);
		});
	}

	public override void ShowUI()
	{
		throw new NotImplementedException();
	}

	public override void CloseUI()
	{
		throw new NotImplementedException();
	}
}
