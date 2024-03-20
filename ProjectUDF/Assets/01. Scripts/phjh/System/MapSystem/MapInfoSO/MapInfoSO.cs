using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "RandomFloorInfoSO", menuName = "SO/Map/RandomFloorInfo")]
public class MapInfoSO : ScriptableObject
{
    public int numberOfRooms; //������ ����

    public List<RoomInfoSO> roomLists; //�� ����Ʈ��
    //[HideInInspector]
    public List<RoomInfoSO> floorRoomInfo; //�̹� ������ ���� ���

    
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
        //���� ������ ������ �ְ�ʹٸ� ������ �ȴ�
        Debug.Log("Susscessful Map Info Generated!");
    }



}
