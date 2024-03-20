using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "RandomFloorInfoSO", menuName = "SO/Map/RandomFloorInfo")]
public class MapInfoSO : ScriptableObject
{
    public int numberOfRooms; //보스방 제외

    public List<RoomInfoSO> roomLists; //방 리스트들
    //[HideInInspector]
    public List<RoomInfoSO> floorRoomInfo; //이번 층에서 나올 방들

    
    public MapInfoSO CloneAndSetting()
    {
        var clone = Instantiate(this);
        clone.GenerateRandomMapInfoSO();
        Debug.Log(clone);
        return clone;
    }

    private void GenerateRandomMapInfoSO()
    {
        Debug.Log("Start Map Info Generating");
        for(int i = 0; i < numberOfRooms; i++)
        {
            int rand = Random.Range(0, roomLists.Count);
            while(i !=0 && floorRoomInfo[i-1].id == roomLists[rand].id)
            {
                rand = Random.Range(0, roomLists.Count);
            }
            floorRoomInfo.Add(roomLists[rand].CloneAndSetting());
        }
        //여기 보스방 고정을 넣고싶다면 넣으면 된다
        Debug.Log("Susscessful Map Info Generated!");
    }



}
