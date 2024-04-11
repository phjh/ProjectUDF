using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingPair
{
    [Tooltip("Pooling type�����ϱ�")]
    public PoolObjectListEnum enumtype;
    [Tooltip("PoolableMono�� ��ӵ� ������Ʈ �ֱ�")]
    public PoolableMono prefab;
    [Header("�ߺ��Ǹ� �ȵǴ� �߿��� �Լ�")]
    public string name;
    [Tooltip("������ ������Ʈ ����")]
    public int count = 15;
    [Tooltip("������ ������Ʈ ����")]
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
    [Tooltip("PoolableMono�� ��ӵ� ������Ʈ �ֱ�")]
    public PoolableMono prefab;
    [Header("�ߺ��Ǹ� �ȵǴ� �߿��� �Լ�")]
    public string name;
    [Tooltip("������ ������Ʈ ����")]
    public int count = 5;
    [Tooltip("������ ������Ʈ ����")]
    public string description;

    public override string ToString()
    {
        return $"Type : {enumtype} \n Prefab : {prefab.name} \n Name : {name} , count : {count} \n Description : {description}";
    }
}

[Serializable]
public class UIPoolingPair
{
    [Tooltip("Pooling type�����ϱ�")]
    public PoolUIListEnum enumtype;
    [Tooltip("PoolableMono�� ��ӵ� ������Ʈ �ֱ�")]
    public PoolableMono prefab;
    [Header("�ߺ��Ǹ� �ȵǴ� �߿��� �Լ�")]
    public string name;
    [Tooltip("������ ������Ʈ ����")]
    public int count = 20;
    [Tooltip("������ ������Ʈ ����")]
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
