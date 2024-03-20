using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PushButtonEffect : MonoBehaviour
{
	public void OnPush()
	{
		var seq = DOTween.Sequence();

		seq.Append(transform.DOScale(0.75f, 0.1f));
		seq.Append(transform.DOScale(1.05f, 0.1f));
		seq.Append(transform.DOScale(1f, 0.1f));

		seq.Play().OnComplete(() => {
			//������ �۵��� ������ �� ������ ��
		});
	}

	public void DisableButton()
	{
		gameObject.SetActive(false);
	}
}