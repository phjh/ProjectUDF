using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PushEffect : MonoBehaviour
{
	public void OnPush()
	{
		GetComponent<Button>().interactable = false;
		var seq = DOTween.Sequence();

		seq.Append(transform.DOScale(0.75f, 0.1f));
		seq.Append(transform.DOScale(2.9f, 0.1f));
		seq.Append(transform.DOScale(2.4f, 0.2f));

		seq.Play().OnComplete(() => {
			DisableButton();
		});
	}

	public void DisableButton()
	{
		gameObject.SetActive(false);
		transform.localScale = new Vector3(2.4f, 2.4f, 2.4f);
		GetComponent<Button>().interactable = true;
	}
}