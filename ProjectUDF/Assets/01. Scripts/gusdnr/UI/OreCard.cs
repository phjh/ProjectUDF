using DG.Tweening;
using System;
using UnityEngine;

public class OreCard : MonoBehaviour
{
	protected bool isActive;

	[SerializeField] private GameObject ResetBtn;

	private void Awake()
	{
		DOTween.Init();
		transform.localScale = Vector3.one * 0.1f;
		UIManager.Instance.Cards.Add(this);
		gameObject.SetActive(false);
		ResetBtn.SetActive(false);
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
			UIManager.Instance.HideCards();
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
		});
	}
}
