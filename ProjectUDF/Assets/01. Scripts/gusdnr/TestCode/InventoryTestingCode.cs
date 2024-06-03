using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTestingCode : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			UIManager.Instance.ShowMining();
		}
	}
}
