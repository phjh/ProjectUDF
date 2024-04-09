using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingPair
{
    [TooltipAttribute("Pooling type�����ϱ�")]
    public PoolObjectListEnum enumtype;
    [TooltipAttribute("PoolableMono�� ��ӵ� ������Ʈ �ֱ�")]
    public PoolableMono prefab;
    [Header("�ߺ��Ǹ� �ȵǴ� �߿��� �Լ�")]
    public string name;
    [TooltipAttribute("������ ������Ʈ ����")]
    public int count;
    [TooltipAttribute("������ ������Ʈ ����")]
    public string description;

    public override string ToString()
    {
        return $"Type : {enumtype} \n Prefab : {prefab.name} \n Name : {name} , count : {count} \n Description : {description}";
    }
}

[Serializable]
public class EffectPoolingPair
{
    [Tooltip("Pooling type�����ϱ�")]
    public PoolEffectListEnum enumtype;
    [TooltipAttribute("PoolableMono�� ��ӵ� ������Ʈ �ֱ�")]
    public PoolableMono prefab;
    [Header("�ߺ��Ǹ� �ȵǴ� �߿��� �Լ�")]
    public string name;
    [TooltipAttribute("������ ������Ʈ ����")]
    public int count;
    [TooltipAttribute("������ ������Ʈ ����")]
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
