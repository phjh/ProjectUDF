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
			//시퀀스 작동이 끝났을 때 실행할 것
		});
	}

	public void DisableButton()
	{
		gameObject.SetActive(false);
	}
}