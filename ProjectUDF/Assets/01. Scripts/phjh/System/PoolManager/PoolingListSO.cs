using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingPair
{
    [Tooltip("Pooling type세팅하기")]
    public PoolObjectListEnum enumtype;
    [Tooltip("PoolableMono가 상속된 오브젝트 넣기")]
    public PoolableMono prefab;
    [Header("중복되면 안되는 중요한 함수")]
    public string name;
    [Tooltip("생성할 오브젝트 개수")]
    public int count = 15;
    [Tooltip("생성할 오브젝트 설명")]
    public string description;

    public override string ToString()
    {
        return $"Type : {enumtype} \n Prefab : {prefab.name} \n Name : {name} , count : {count} \n Description : {description}";
    }
}

[Serializable]
public class EffectPoolingPair
{
    [Tooltip("Pooling type세팅하기")]
    public PoolEffectListEnum enumtype;
    [Tooltip("PoolableMono가 상속된 오브젝트 넣기")]
    public PoolableMono prefab;
    [Header("중복되면 안되는 중요한 함수")]
    public string name;
    [Tooltip("생성할 오브젝트 개수")]
    public int count = 5;
    [Tooltip("생성할 오브젝트 설명")]
    public string description;

    public override string ToString()
    {
        return $"Type : {enumtype} \n Prefab : {prefab.name} \n Name : {name} , count : {count} \n Description : {description}";
    }
}

[Serializable]
public class UIPoolingPair
{
    [Tooltip("Pooling type세팅하기")]
    public PoolUIListEnum enumtype;
    [Tooltip("PoolableMono가 상속된 오브젝트 넣기")]
    public PoolableMono prefab;
    [Header("중복되면 안되는 중요한 함수")]
    public string name;
    [Tooltip("생성할 오브젝트 개수")]
    public int count = 20;
    [Tooltip("생성할 오브젝트 설명")]
    public string description;

    public override string ToString()
    {
        return $"Type : {enumtype} \n Prefab : {prefab.name} \n Name : {name} , count : {count} \n Description : {description}";
    }
}

[CreateAssetMenu(menuName = "SO/Pool/list")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingPair> PoolObjectLists;
    public List<EffectPoolingPair> PoolEffectLists;
    public List<UIPoolingPair> PoolUILists;
}
