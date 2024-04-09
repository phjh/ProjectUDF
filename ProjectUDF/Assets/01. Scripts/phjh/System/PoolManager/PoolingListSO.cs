using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingPair
{
    [TooltipAttribute("Pooling type세팅하기")]
    public PoolObjectListEnum enumtype;
    [TooltipAttribute("PoolableMono가 상속된 오브젝트 넣기")]
    public PoolableMono prefab;
    [Header("중복되면 안되는 중요한 함수")]
    public string name;
    [TooltipAttribute("생성할 오브젝트 개수")]
    public int count;
    [TooltipAttribute("생성할 오브젝트 설명")]
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
    [TooltipAttribute("PoolableMono가 상속된 오브젝트 넣기")]
    public PoolableMono prefab;
    [Header("중복되면 안되는 중요한 함수")]
    public string name;
    [TooltipAttribute("생성할 오브젝트 개수")]
    public int count;
    [TooltipAttribute("생성할 오브젝트 설명")]
    public string description;

    public override string ToString()
    {
        return $"Type : {enumtype} \n Prefab : {prefab.name} \n Name : {name} , count : {count} \n Description : {description}";
    }
}


[CreateAssetMenu(menuName = "SO/Pool/list")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingPair> PoolingLists;
    public List<EffectPoolingPair> EffectLists;
}
