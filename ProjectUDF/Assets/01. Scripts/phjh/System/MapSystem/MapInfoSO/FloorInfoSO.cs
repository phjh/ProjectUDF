using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;


[CreateAssetMenu(fileName = "FloorInfoSO", menuName = "SO/Map/FloorInfo")]
public class FloorInfoSO : ScriptableObject
{
    [Header("�� ����")]
    public int ReachToBoss = 0;

    [Header("�� ���")]
    public List<RoomInfoSO> roomList; //�� ����Ʈ��
	public RoomInfoSO BossRoom;
    //[HideInInspector]
    public List<RoomInfoSO> floorRoomInfo; //�̹� ������ ���� ���

    public FloorInfoSO CloneAndSetting(bool isRandom = false)
    {
        var clone = Instantiate(this);
        clone.GenerateMapInfoSO(isRandom);
        Debug.Log(clone);
        return clone;
    }

    private void GenerateMapInfoSO(bool isRandom =false)
    {
        Debug.Log("Start Map Info Generating");
        List<int> SelectedRooms = new List<int>(); // �ֱ� ���õ� ���� ID�� ������ ����Ʈ
		for (int i = 0; i < ReachToBoss; i++)
		{
			int roomNumber = IsSelectedChecking(SelectedRooms);

			// �� ���� �߰�
			if(isRandom) floorRoomInfo.Add(roomList[roomNumber].CloneAndSettingRandom());
			else floorRoomInfo.Add(roomList[roomNumber].CloneAndSetting());

			// ����Ʈ ������Ʈ
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
