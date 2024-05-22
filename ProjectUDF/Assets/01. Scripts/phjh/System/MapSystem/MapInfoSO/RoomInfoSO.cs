using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonsterInfo 
{
    public GameObject monsterObj;
    public Vector2 monsterPos;
}

[Serializable]
public enum Exit
{
    Down=0,
    Right,
    Up,
    Left
}

[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")] 
public class RoomInfoSO : ScriptableObject
{
    [Header("방 정보들")]
    public int id;
    public int monsterWaves;
    public GameObject MapPrefab;
    public float timeLimit = 100;

    [Header("웨이브별 나오는 몬스터 수")]
    public List<int> numberOfMonsters;

    [Header("몬스터 리스트")]
    public List<GameObject> spawnMonsterList; //소환 가능성있는 몬스터들
    //[HideInInspector]
    //건들면 안됨
    public List<MonsterInfo> spawnMonsters; //소환될 몬스터들

    [Tooltip("출구 위치들")]
    public List<Exit> exit;

    public RoomInfoSO CloneAndSetting()
    {
        var thisMap = Instantiate(this);
        Debug.Log(thisMap);
        return thisMap;
    }

    public RoomInfoSO CloneAndSettingRandom()
    {
        var thisMap = Instantiate(this);
		thisMap.GenerateRandomMonsterInfo();
        //a.SetExitPoint();
        if (exit.Count == 0)
        {
            Debug.LogError("exit room is not exist");
        }
        return thisMap;
    }

    private void GenerateRandomMonsterInfo()
    {
        if (numberOfMonsters.Count != monsterWaves)
        {
            Debug.LogError("값 틀림 : monsterWaves");
            return;
        }
        for (int i = 0; i < monsterWaves; i++)
        {
            int a = numberOfMonsters[i];
            for (int j = 0; j < a; j++)
            {
                int rand = UnityEngine.Random.Range(0, spawnMonsterList.Count);
                MonsterInfo monster;
                monster.monsterObj = spawnMonsterList[rand];
                monster.monsterPos = new Vector2(UnityEngine.Random.Range(-11, 11), UnityEngine.Random.Range(-6, 3));
                spawnMonsters.Add(monster);
            }
        }
    }

    public void DebugMonsters()
    {
        int i = 0;
        for(int a = 0; a < numberOfMonsters.Count; a++)
        {
            string str = "\n";
            str += this.name + "\n";
            int l = numberOfMonsters[a];
            for(int b = 0; b < l; b++)
            {
                str += spawnMonsters[i] + " | ";
                i++;
            }
            Debug.Log($"{str} __ {a}번째 웨이브 몹");
        }
    }

    //public void SetExitPoint()
    //{
    //    int rand = UnityEngine.Random.Range(0, 4);
    //    exit = (Exit)rand;
    //}

}