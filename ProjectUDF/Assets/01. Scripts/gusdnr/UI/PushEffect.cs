using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PushEffect : MonoBehaviour
{
	public void OnPush()
	{
		var seq = DOTween.Sequence();

		seq.Append(transform.DOScale(0.75f, 0.1f));
		seq.Append(transform.DOScale(1.2f, 0.2f));
		seq.Append(transform.DOScale(1f, 0.1f));

		seq.Play().OnComplete(() => {
			DisableButton();
			transform.localScale = new Vector3(2.4f, 2.4f, 2.4f);
		});
	}

	public void DisableButton()
	{
		gameObject.SetActive(false);
	}
}