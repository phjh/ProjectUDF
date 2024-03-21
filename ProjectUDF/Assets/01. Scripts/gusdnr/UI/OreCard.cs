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

		// DOTween 함수를 차례대로 수행하게 해줍니다.
		var seq = DOTween.Sequence();

		// DOScale 의 첫 번째 파라미터는 목표 Scale 값, 두 번째는 시간입니다.
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


		// OnComplete 는 seq 에 설정한 애니메이션의 플레이가 완료되면
		// { } 안에 있는 코드가 수행된다는 의미입니다.
		// 여기서는 닫기 애니메이션이 완료된 후 객체를 비활성화 합니다.
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
