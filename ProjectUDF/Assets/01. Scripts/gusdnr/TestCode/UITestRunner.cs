using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestRunner : MonoBehaviour
{

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			UIManager.Instance.ShowMining();
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			UIManager.Instance.ShowPocket();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			UIManager.Instance.ClosePocket();
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
