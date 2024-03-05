using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ore Inventory List", menuName = "SO/Player/OreInventory")]
public class OreInventory : ScriptableObject
{
    public List<int> NormalOreList = new List<int>(5);
    public List<int> UpgradeOreList = new List<int>(4);

	private void Awake()
	{
        for(int i = 0; i < NormalOreList.Count; i++)
        {
            NormalOreList[i] = 0;
            UpgradeOreList[i] = 0;
        }
    }

	public void EnterNomalOre(Stats statName)
    {
		int num = (int)statName;
		NormalOreList[num] += 1;
        if (NormalOreList[num] == 3 && num != 4)
        {
            EnterUpgradeOre(num);
            NormalOreList[num] = 0;
        }
	}

    public void EnterUpgradeOre(int statNumber)
    {
        UpgradeOreList[statNumber] += 1;
        
    }

}
