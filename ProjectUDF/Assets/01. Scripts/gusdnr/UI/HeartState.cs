using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIHeartState
{
	Empty = 0,
	Full = 1
}

public class HeartState : MonoBehaviour
{
	[SerializeField] private Sprite full, empty;
	private Image heartImage;

	private void Awake()
	{
		heartImage = GetComponent<Image>();
	}

	public void SetHeartImage(UIHeartState status)
	{
		switch (status)
		{
			case UIHeartState.Empty:
				heartImage.sprite = empty;
				break;
			case UIHeartState.Full:
				heartImage.sprite = full;
				break;
		}
	}
}
