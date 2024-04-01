using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolingType
{
    MiniGolem,
    StoneToad,
    ToadBullet,
    item,
}

[Serializable]
public struct PoolingPair
{
    public PoolingType type;
    public PoolableMono prefab;
    public int count;
    public string name;
    public string description;
}

[Serializable]
public struct EffectPoolingPair
{
    public EffectPoolingType type;
    public EffectPoolableMono prefab;
    public int count;
    public string name;
    public string description;
}

[CreateAssetMenu(menuName = "SO/Pool/list")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingPair> PoolObjtectList;
    public List<EffectPoolingPair> PoolEffectLists;
}
