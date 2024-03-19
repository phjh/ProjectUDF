using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")]
public class RoomInfoSO : ScriptableObject
{
    [Header("방 정보들")]
    public int id;
    public int monsterWaves;
    public List<int> numberOfMonsters;

    [Header("몬스터 리스트")]
    public List<GameObject> spawnMonsterList;
    [HideInInspector]
    public List<GameObject> spawnMonster;
    //건들면 안됨

    public RoomInfoSO CloneAndSetting()
    {
        var a = Instantiate(this);
        a.GenerateRandomMonsterSpawnInfo();
        return a;
    }

    private void GenerateRandomMonsterSpawnInfo()
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

}