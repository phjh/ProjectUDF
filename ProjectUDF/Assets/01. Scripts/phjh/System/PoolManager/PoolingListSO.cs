using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolingType
{
    Monster,
    monster2,
    monster3,
    item,
    ChargeAttackEffect,

}

[Serializable]
public struct PoolingPair
{
    public PoolingType type;
    public PoolableMono prefab;
    public int count;
}

[CreateAssetMenu(menuName = "SO/Pool/list")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingPair> list;
}
