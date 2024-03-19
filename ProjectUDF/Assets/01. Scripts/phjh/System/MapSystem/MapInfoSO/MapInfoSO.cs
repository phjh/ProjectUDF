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
    
    [Header("���� ����Ʈ")]
    public List<GameObject> spawnMonsterList;
    [Header("������ ���� ����Ʈ")]
    public List<GameObject> spawnMonster;
    //�ǵ�� �ȵ�

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
            Debug.LogError("�� Ʋ�� : monsterWaves");
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
    public int numberOfRooms; //������ ����

    public List<Room> roomLists; //�� ����Ʈ��
    public List<Room> floorRoomInfo; //�̹� ������ ���� ���

    
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
        //���� ������ ������ �ְ�ʹٸ� ������ �ȴ�
        Debug.Log("Susscessful Map Info Generated!");
    }



}
