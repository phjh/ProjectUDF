using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OreData", menuName = "SO/DataSO/OreSO")]
public class OreSO : ScriptableObject
{
    [Header("Ore Infomation")]
    public Sprite OreSprite = null;
    public string OreName = "Empty";
    public string OreDesc = "Empty";

    [Header("Ore Increase Stat")]
    public Stats stats;
    public int value;
    public int valuePersent;
}
