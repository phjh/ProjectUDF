using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestRunner : MonoBehaviour
{
	void Start()
    {
		
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Z))
		{
			UIManager.Instance.ShowCards();
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			InventoryUIManager.Instance.Show();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			InventoryUIManager.Instance.Close();
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			TimeManager.Instance.StartTimer();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			TimeManager.Instance.StopTimer();
		}
	}
}
