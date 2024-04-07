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
public class PoolingPair
{
    [Header("PoolableMono가 상속된 오브젝트 넣기")]
    public PoolableMono prefab;
    [Header("중복되면 안되는 중요한 함수들")]
    public int ID;
    public string name;
    [Header("생성할 오브젝트 개수와 설명")]
    public int count;
    public string description;

    public PoolingPair GetPoolingPair()
    {
        return this;
    }

    public override string ToString()
    {
        return $"Prefab : {prefab.name} \n Id : {ID}, Name : {name} \n count : {count} \n Description : {description}";
    }
}


[CreateAssetMenu(menuName = "SO/Pool/list")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingPair> PoolingLists;
}
