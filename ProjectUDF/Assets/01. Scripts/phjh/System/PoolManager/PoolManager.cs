using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class PoolObjectIdentifier
//{
//    public int ID;
//    public string Name;
//    public PoolableMono connectedPrefab;

//    public PoolObjectIdentifier(PoolingPair pair) 
//    {
//        ID = pair.ID;
//        Name = pair.name;
//        connectedPrefab = pair.prefab;
//    }

//    public static PoolObjectIdentifier GetIdentifier(Dictionary<PoolObjectIdentifier, Pool<PoolableMono>> map,  PoolingPair pair)
//    {
//        PoolObjectIdentifier identifier = new PoolObjectIdentifier(pair);
//        foreach(PoolObjectIdentifier mapIdentifier in map.Keys)
//        {
//            if (mapIdentifier.Equals(identifier))
//                return mapIdentifier;
//            Debug.LogWarning($"map key : {mapIdentifier.ToString()} \n pair key : {identifier.ToString()}");
//        }
//        Debug.LogError("This Pair is not exist at Dictionary! \n  pair : " + pair.ToString());
//        return null;
//    }

//    public static PoolObjectIdentifier GetIdentifier(Dictionary<PoolObjectIdentifier, Pool<PoolableMono>> map, int id)
//    {
//        foreach (PoolObjectIdentifier identifier in map.Keys)
//        {
//            if (identifier.ID == id)
//                return identifier;
//        }
//        Debug.LogError("this id was not Idenified!! \n id : " + id);
//        return null;
//    }

//    public static PoolObjectIdentifier GetIdentifier(Dictionary<PoolObjectIdentifier, Pool<PoolableMono>> map, string name)
//    {
//        foreach (PoolObjectIdentifier identifier in map.Keys)
//        {
//            if (identifier.Name == name)
//                return identifier;
//        }
//        Debug.LogError("this id was not Idenified!! \n Name : " + name);
//        return null;
//    }

//    public override string ToString()
//    {
//        return $"id : {ID}, name : {Name}";
//    }

//    public bool Equals(PoolObjectIdentifier identifier)
//    {
//        return ID == identifier.ID && Name == identifier.Name;
//    }
//}

public class PoolManager 
{
    public static PoolManager Instance;

    private Dictionary<PoolObjectListEnum, Pool<PoolableMono>> ObjectPoolingList = new();
    private Dictionary<PoolEffectListEnum, Pool<PoolableMono>> EffectPoolingList = new();
    private Dictionary<PoolUIListEnum, Pool<PoolableMono>> UIPoolingList = new();

    #region Improved PoolManager

    public void CreatePool(PoolingPair pair, Transform parent)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(pair.prefab, parent, pair.count);
        ObjectPoolingList.Add(pair.enumtype, pool);
    }

    public void CreatePool(EffectPoolingPair pair, Transform parent)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(pair.prefab, parent, pair.count);
        EffectPoolingList.Add(pair.enumtype, pool);
    }

    public void CreatePool(UIPoolingPair pair, Transform parent)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(pair.prefab, parent, pair.count);
        UIPoolingList.Add(pair.enumtype, pool);
    }

    public PoolableMono Pop(PoolObjectListEnum enumlist)
    {
        if (!ObjectPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }
        PoolableMono item = ObjectPoolingList[enumlist].Pop();
        item.ResetPoolingItem();
        return item;
    }

    public PoolableMono Pop(PoolEffectListEnum enumlist)
    {
        if (!EffectPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError($"Prefab - {enumlist.ToString()} doesnt exist on pool");
            return null;
        }
        PoolableMono item = EffectPoolingList[enumlist].Pop();
        item.ResetPoolingItem();
        return item;
    }

    public PoolableMono Pop(PoolUIListEnum enumlist)
    {
        if (!UIPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }
        PoolableMono item = UIPoolingList[enumlist].Pop();
        //item.ResetPoolingItem();
        return item;
    }

    public PoolableMono Pop(PoolObjectListEnum enumlist, Transform parent)
    {
        if (!ObjectPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }
        PoolableMono item = ObjectPoolingList[enumlist].Pop();
        item.transform.SetParent(parent);
        item.ResetPoolingItem();
        return item;
    }

    public PoolableMono Pop(PoolEffectListEnum enumlist, Transform parent)
    {
        if (!EffectPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }
        PoolableMono item = EffectPoolingList[enumlist].Pop();
        item.transform.SetParent(parent);
        item.ResetPoolingItem();
        return item;
    }

    public void Push(PoolableMono obj, PoolObjectListEnum enumlist)
    {
        ObjectPoolingList[enumlist].Push(obj);
    }

    public void Push(PoolableMono obj, PoolEffectListEnum enumlist)
    {
        EffectPoolingList[enumlist].Push(obj);
    }

    public void Push(PoolableMono obj, PoolUIListEnum enumlist)
    {
        UIPoolingList[enumlist].Push(obj);
    }

    #endregion


    #region Fixed PoolManager

    //private Dictionary<PoolObjectIdentifier, Pool<PoolableMono>> ObjectPoolingList = new();
    //private Dictionary<PoolObjectIdentifier, Pool<PoolableMono>> EffectPoolingList = new();
    ////위에 꺼로 바꾸기

    ////[SerializeField]
    ////private Dictionary<PoolingType, Pool<PoolableMono>> _pools = new Dictionary<PoolingType, Pool<PoolableMono>>();
    ////[SerializeField]
    ////private Dictionary<EffectPoolingType, EffectPool<EffectPoolableMono>> _effectPools = new Dictionary<EffectPoolingType, EffectPool<EffectPoolableMono>>();

    //public void CreatePool(PoolingPair pair, Transform parent)
    //{
    //    PoolObjectIdentifier identifier = new PoolObjectIdentifier(pair);
    //    Pool<PoolableMono> pool = new Pool<PoolableMono>(pair.prefab, parent, pair.count);
    //    ObjectPoolingList.Add(identifier, pool);
    //}

    ////Root~~ 메서드는 ~~ 메서드들에서 호출하는 그 메서드의 뿌리같은 메서드다
    //PoolableMono RootPop(PoolObjectIdentifier identifier)
    //{
    //    if (!ObjectPoolingList.ContainsKey(identifier))  //Identifier 검사
    //    {
    //        Debug.LogError($"PoolManager - Identifier : {identifier} is null");
    //        return null;
    //    }
    //    PoolableMono obj = ObjectPoolingList[identifier].Pop();
    //    obj.ResetPooingItem();
    //    return obj;
    //}

    //public PoolableMono Pop(int id)
    //{
    //    PoolObjectIdentifier identifier = PoolObjectIdentifier.GetIdentifier(ObjectPoolingList, id);
    //    return RootPop(identifier);
    //}

    //public PoolableMono Pop(string name)
    //{
    //    PoolObjectIdentifier identifier = PoolObjectIdentifier.GetIdentifier(ObjectPoolingList, name);
    //    return RootPop(identifier);
    //}

    //public PoolableMono Pop(PoolingPair pair)
    //{
    //    PoolObjectIdentifier identifier = PoolObjectIdentifier.GetIdentifier(ObjectPoolingList, pair);
    //    return RootPop(identifier);
    //}   

    //public void Push(PoolingPair pair)
    //{
    //    PoolObjectIdentifier identifer = PoolObjectIdentifier.GetIdentifier(ObjectPoolingList, pair);

    //    if (identifer.connectedPrefab != pair.prefab)
    //        Debug.LogError($"Wrong PoolingPair! \n identifier prefab : {identifer.connectedPrefab}\n Poolable obj : {pair.prefab}");

    //    ObjectPoolingList[identifer].Push(pair.prefab);
    //}   

    //public void Push(PoolableMono obj,  string name)
    //{
    //    PoolObjectIdentifier identifer = PoolObjectIdentifier.GetIdentifier(ObjectPoolingList, name);

    //    if (identifer.connectedPrefab != obj)
    //        Debug.LogError($"Wrong name! \n identifier prefab : {identifer.connectedPrefab}\n Poolable obj : {obj}");

    //    ObjectPoolingList[identifer].Push(obj);
    //}

    //public void Push(PoolableMono obj,  int id)
    //{
    //    PoolObjectIdentifier identifer = PoolObjectIdentifier.GetIdentifier(ObjectPoolingList, id);

    //    if (identifer.connectedPrefab != obj)
    //        Debug.LogError($"Wrong id! \n identifier prefab : {identifer.connectedPrefab}\n Poolable obj : {obj}");

    //    ObjectPoolingList[identifer].Push(obj);
    //}


    #endregion


    #region Obsolete PoolManager

    //public void CreatePool(PoolableMono prefab, PoolingType poolingType, int count = 10)
    //{
    //    Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, poolingType, _trmParent, count);
    //    Debug.Log('a');
    //    _pools.Add(poolingType, pool);
    //}

    //public PoolableMono Pop(PoolingType type)
    //{
    //    if (!_pools.ContainsKey(type))
    //    {
    //        Debug.LogError("Prefab doesnt exist on pool");
    //        return null;
    //    }
    //    PoolableMono item = _pools[type].Pop();
    //    item.ResetPooingItem();
    //    return item;
    //}

    //public void Push(PoolableMono obj, bool resetParent = false)
    //{
    //    if(resetParent)
    //        obj.transform.parent = _trmParent;
    //    _pools[obj.poolingType].Push(obj);
    //}

    //public void CreatePool(EffectPoolableMono prefab, EffectPoolingType poolingType, int count = 10)
    //{
    //    EffectPool<EffectPoolableMono> pool = new EffectPool<EffectPoolableMono>(prefab, poolingType, _trmParent, count);
    //    Debug.Log('a');
    //    _effectPools.Add(poolingType, pool);
    //}

    //public EffectPoolableMono Pop(EffectPoolingType type)
    //{
    //    if (!_effectPools.ContainsKey(type))
    //    {
    //        Debug.LogError("Prefab doesnt exist on pool");
    //        return null;
    //    }
    //    EffectPoolableMono item = _effectPools[type].Pop();
    //    item.ResetPooingItem();
    //    return item;
    //}


    //public void Push(EffectPoolableMono obj, bool resetParent = false)
    //{
    //    if (resetParent)
    //        obj.transform.parent = _trmParent;
    //    _effectPools[obj.poolingType].Push(obj);
    //}

    #endregion

}