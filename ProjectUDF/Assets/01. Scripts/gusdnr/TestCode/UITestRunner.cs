using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestRunner : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
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
	}
}
