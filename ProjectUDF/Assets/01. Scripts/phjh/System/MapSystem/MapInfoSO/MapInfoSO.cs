using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum OreList
{
    StrengthOre = 0,
    LuckyOre = 1,
    MoveSpeedOre = 2,
    AttackSpeedOre = 3,
    HealOre = 4,
    End =5
}


[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")]
public class Room : ScriptableObject
{
    public int id;
    public int monsterWaves;
    
    public List<int> numberOfMonsters;
    
    [Header("몬스터 리스트")]
    public List<GameObject> spawnMonsterList;
    [Header("생성될 몬스터 리스트")]
    public List<GameObject> spawnMonster;
    //건들면 안됨

    public Room CloneAndSetting()
    {
        var a = Instantiate(this);
        a.GenerateRandomMonsterSpawnInfo();
        return a;
    }

    private async void GenerateRandomMonsterSpawnInfo()
    {
        if(numberOfMonsters.Count != monsterWaves)
        {
            Debug.LogError("값 틀림 : monsterWaves");
            return;
        }
        await Task.Run(() =>
        {
            for (int i = 0; i < monsterWaves; i++)
            {
                int a = numberOfMonsters[i];
                for (int j = 0; j < a; j++)
                {
                    int rand = Random.Range(0,spawnMonsterList.Count);
                    spawnMonster.Add(spawnMonsterList[rand]);
                }
            }
        });
    }
    
}

[CreateAssetMenu(fileName = "RandomFloorInfoSO", menuName = "SO/Map/RandomFloorInfo")]
public class MapInfoSO : ScriptableObject
{
    public int numberOfRooms; //보스방 제외

    public List<Room> roomLists; //방 리스트들
    public List<Room> floorRoomInfo; //이번 층에서 나올 방들

    
    public MapInfoSO CloneAndSetting()
    {
        var clone = Instantiate(this);
        clone.GenerateRandomMapInfoSO();
        return clone;
    }

    private async void GenerateRandomMapInfoSO()
    {
        Debug.Log("Start Map Info Generating");
        await Task.Run(() =>
        {
            for(int i = 0; i < numberOfRooms; i++)
            {
                int rand = Random.Range(0, roomLists.Count);
                floorRoomInfo.Add(roomLists[rand].CloneAndSetting());
            }
        });
        //여기 보스방 고정을 넣고싶다면 넣으면 된다
        Debug.Log("Susscessful Map Info Generated!");
    }



}
