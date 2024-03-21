using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")]
public class RoomInfoSO : ScriptableObject
{
    [Header("방 정보들")]
    public int id;
    public int monsterWaves;
    public GameObject MapPrefab;

    [Header("웨이브별 나오는 몬스터 수")]
    public List<int> numberOfMonsters;

    [Header("몬스터 리스트")]
    public List<GameObject> spawnMonsterList; //소환 가능성있는 몬스터들
    //[HideInInspector]
    //건들면 안됨
    public List<GameObject> spawnMonster; //소환될 몬스터들

    public RoomInfoSO CloneAndSetting()
    {
        var a = Instantiate(this);
        a.GenerateMonsterInfo();
        Debug.Log(a);
        return a;
    }

    public RoomInfoSO CloneAndSettingRandom()
    {
        var a = Instantiate(this);
        a.GenerateRandomMonsterInfo();
        Debug.Log(a);
        return a;
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
                int rand = Random.Range(0, spawnMonsterList.Count);
                spawnMonster.Add(spawnMonsterList[rand]); 
            }
        }
    }

    public void GenerateMonsterInfo()
    {

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
                str += spawnMonster[i] + " | ";
                i++;
            }
            Debug.Log($"{str} __ {a}번째 웨이브 몹");
        }
    }

}