using DG.Tweening;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
	private void Awake()
	{
		DOTween.Init();
		// transform �� scale ���� ��� 0.1f�� �����մϴ�.
		transform.localScale = Vector3.one * 0.1f;
		UIManager.Instance.Cards.Add(gameObject);
		gameObject.SetActive(false);
	}

	public void Show()
	{
		gameObject.SetActive(true);

		// DOTween �Լ��� ���ʴ�� �����ϰ� ���ݴϴ�.
		var seq = DOTween.Sequence();

		// DOScale �� ù ��° �Ķ���ʹ� ��ǥ Scale ��, �� ��°�� �ð��Դϴ�.
		seq.Append(transform.DOScale(1.3f, 0.3f));
		seq.Append(transform.DOScale(1f, 0.2f));

		seq.Play();
	}

	public void HideDefault()
	{
		var seq = DOTween.Sequence();

		transform.localScale = Vector3.one * 0.1f;

		seq.Append(transform.DOScale(1f, 0.1f));
		seq.Append(transform.DOScale(0.2f, 0.2f));


		seq.Play().OnComplete(() =>
		{
			UIManager.Instance.Cards.Remove(gameObject);
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
		// ���⼭�� �ݱ� �ִϸ��̼��� �Ϸ�� �� ��ÿ�� ��Ȱ��ȭ �մϴ�.
		seq.Play().OnComplete(() =>
		{
            UIManager.Instance.Cards.Remove(gameObject);
			if(UIManager.Instance.Cards != null) UIManager.Instance.Cards.ForEach(x =>
			{
				x.GetComponent<PanelHandler>().HideDefault();
			});
			gameObject.SetActive(false);
        });
	}
}
