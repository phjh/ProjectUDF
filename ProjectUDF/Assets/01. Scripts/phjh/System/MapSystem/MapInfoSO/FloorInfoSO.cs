using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FloorInfoSO", menuName = "SO/Map/FloorInfo")]
public class FloorInfoSO : ScriptableObject
{
    [Header("층 변수")]
    public int ReachToBoss = 0;

    [Header("방 목록")]
    public List<RoomInfoSO> roomList; //방 리스트들
	public RoomInfoSO BossRoom;
    public List<RoomInfoSO> floorRoomInfo; //이번 층에서 나올 방들

    public FloorInfoSO CloneAndSetting(bool isRandom = false)
    {
        var clone = Instantiate(this);
        clone.GenerateMapInfoSO(isRandom);
        Debug.Log(clone);
        return clone;
    }

    private void GenerateMapInfoSO(bool isRandom)
    {
		if (isRandom)
			floorRoomInfo.Clear();

        Debug.Log("Start Map Info Generating");
        List<int> SelectedRooms = new List<int>(); // 최근 선택된 방의 ID를 저장할 리스트
		for (int i = 0; i < ReachToBoss; i++)
		{
			int roomNumber = IsSelectedChecking(SelectedRooms);

			// 방 정보 추가
			if(isRandom) floorRoomInfo.Add(roomList[roomNumber].CloneAndSetting(true));
			else floorRoomInfo.Add(roomList[i - (roomList.Count / i * roomList.Count)].CloneAndSetting(false));

			// 리스트 업데이트
			SelectedRooms.Add(roomList[roomNumber].id);
			if (SelectedRooms.Count > 3)
			{
				SelectedRooms.RemoveAt(0);
			}
		}
		floorRoomInfo.Add(BossRoom);
		Debug.Log("Susscessful Map Info Generated!");
    }

    private int IsSelectedChecking(List<int> SelectedList)
	{
		int rand = Random.Range(0, roomList.Count);
		while (SelectedList.Contains(roomList[rand].id))
		{
			rand = Random.Range(0, roomList.Count);
		}
		return rand;
	}

}
