using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTransition : MonoBehaviour
{
	[SerializeField] private Vector2 MinPos;
	[SerializeField] private Vector2 MaxPos;
	[SerializeField] private float TransitionTime = 5f;
	[SerializeField] private bool isSide = true;

	private RectTransform ImageTransform;
	private Coroutine TransitionCoroutine;

	private void Awake()
	{
		ImageTransform = GetComponent<RectTransform>();
		ImageTransform.localPosition = MinPos;
	}

	private void Update()
	{
		if (TransitionCoroutine == null)
		{
			if (Input.GetKeyDown(KeyCode.O))
			{
				TransitionCoroutine = StartCoroutine(MinToMax());
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				TransitionCoroutine = StartCoroutine(MaxToMin());
			}
		}
	}

	private IEnumerator MinToMax()
	{
		float time = 0f;
		ImageTransform.localPosition = MinPos;
		while(time <= TransitionTime)
		{
			time += Time.deltaTime;
			if (isSide == true)
			{
				ImageTransform.localPosition = new Vector3(Mathf.Lerp(ImageTransform.localPosition.x, MaxPos.x, time), 0, 0);
			}
			else if(isSide == false)
			{
				ImageTransform.localPosition = new Vector3(0, Mathf.Lerp(ImageTransform.localPosition.y, MaxPos.y, time), 0);
			}
			yield return null;
		}
		yield return TransitionCoroutine = null;
	}

	private IEnumerator MaxToMin()
	{
		float time = 0f;
		ImageTransform.localPosition = MaxPos;
		while (time <= TransitionTime)
		{
			time += Time.deltaTime;
			if (isSide == true)
			{
				ImageTransform.localPosition = new Vector3(Mathf.Lerp(ImageTransform.localPosition.x, MinPos.x, time), 0, 0);
			}
			else if (isSide == false)
			{
				ImageTransform.localPosition = new Vector3(0, Mathf.Lerp(ImageTransform.localPosition.y,MinPos.y,  time), 0);
			}
			yield return null;
		}
		yield return TransitionCoroutine = null;
	}
}
