using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class PoolingPair
{
    [Header("Pooling type�����ϱ�")]
    public PoolObjectListEnum poolObjectenum;
    public PoolEffectListEnum poolEffectenum;
    [Header("PoolableMono�� ��ӵ� ������Ʈ �ֱ�")]
    public PoolableMono prefab;
    [Header("�ߺ��Ǹ� �ȵǴ� �߿��� �Լ���")]
    public int ID;
    public string name;
    [Header("������ ������Ʈ ������ ����")]
    public int count;
    public string description;

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
