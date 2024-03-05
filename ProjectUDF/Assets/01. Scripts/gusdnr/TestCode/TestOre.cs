using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOre : MonoBehaviour
{
    public List<MonoOre> OreList;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			int r = Random.Range(0, OreList.Count);
			OreList[r].GetOre();
		}
	}
}
