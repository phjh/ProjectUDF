using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIHeartState
{
	Empty = 0,
	Half = 1,
	Full = 2
}

public class HeartState : MonoBehaviour
{
	[SerializeField] private Sprite full, half, empty;
	private SpriteRenderer heartImage;

	private void Awake()
	{
		heartImage = GetComponent<SpriteRenderer>();
	}

	public void SetHeartImage(UIHeartState status)
	{
		switch (status)
		{
			case UIHeartState.Empty:
				heartImage.sprite = empty;
				break;
			case UIHeartState.Half:
				heartImage.sprite = half;
				break;
			case UIHeartState.Full:
				heartImage.sprite = full;
				break;
		}
	}
}
