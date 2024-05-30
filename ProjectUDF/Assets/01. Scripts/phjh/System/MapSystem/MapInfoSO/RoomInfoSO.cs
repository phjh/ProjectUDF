using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MapDefine;

[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")] 
public class RoomInfoSO : ScriptableObject
{
    [Header("�� ������")]
    public int id;
    public GameObject TileObject;
    [Range(30, 600)]public float timeLimit = 120;

    [Header("Monster Data")]
    public List<WaveData> RoomWaveData;

    [Tooltip("�ⱸ ��ġ")]
    public List<Exit> exits;

    [HideInInspector] public PlacedTileData Obstacle { get; private set; }
    [HideInInspector] public PlacedTileData Decorate { get; private set; }
    private Tilemap ObstacleMap;
    private Tilemap DecorateMap;

	private void OnEnable()
	{
		if (TileObject != null)
		{
		    SetTileMapComponent();
		}
	}

	public RoomInfoSO CloneAndSetting(bool isRandom = false)
    {
        var CloneInfo = Instantiate(this);
		CloneInfo.SetTileMapComponent();
        
        if(isRandom) CloneInfo.GenerateRandomMonsterInfo();

		if (CloneInfo.exits.Count == 0)
		{
			Debug.LogError("exits room is not exist");
		}

		Debug.Assert(CloneInfo == null, $"Success To Clone RoomInfo : ID [{CloneInfo.id}]");
        return CloneInfo;
    }

    private void GenerateRandomMonsterInfo()
    {
        for (int waveCount = 0; waveCount < RoomWaveData.Count; waveCount++)
        {
			int before = UnityEngine.Random.Range(0, RoomWaveData.Count);
			int after = 0;

			do { after = UnityEngine.Random.Range(0, RoomWaveData.Count); }
			while (before != after);

			WaveData tempMonInfo = RoomWaveData[before];
			RoomWaveData[before] = RoomWaveData[after];
			RoomWaveData[after] = tempMonInfo;
		}
    }

	#region Save Tilemap Data Method

	private void SetTileMapComponent() //Ÿ�ϸ� ������Ʈ�� �ް�, �� Ÿ�� �����͸� �����ϴ� �Լ��� �۵���Ű�� ��
	{
		if (TileObject == null)
        {
            Debug.LogError($"{this.name}'s TileObject Is Null");
            return;
        }

		if (TileObject.transform.Find("ObstacleTile").TryGetComponent(out ObstacleMap) == false) Debug.LogError("Can't Get Obstacle Tile!!");
		if(TileObject.transform.Find("DecorateTile").TryGetComponent(out DecorateMap) == false) Debug.LogError("Can't Get Decorate Tile!!");

		if (ObstacleMap != null && DecorateMap != null)
		{
			SaveRoomData(); //Ÿ�� ������ ���� �Լ�
		}
        else
        {
            Debug.Assert(ObstacleMap != null, "ObstacleMap Is Null");
            Debug.Assert(DecorateMap != null, "DecorateMap Is Null");
        }
	}

	private void SaveRoomData()
    {
        Obstacle = SavePlacedTIleData(ObstacleMap); //������ ��ȯ �Ҵ�
        Decorate = SavePlacedTIleData(DecorateMap); //������ ��ȯ �Ҵ�
    }

    private PlacedTileData SavePlacedTIleData(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int PlacedPos = new Vector3Int();

        PlacedTileData TempContainer = new PlacedTileData(); //��ġ Ÿ�� ������
        for(int x = bounds.min.x; x < bounds.max.x; x++) //�ٿ���� X ����ŭ
        {
            for(int y = bounds.min.y; y < bounds.max.y; y++) //�ٿ���� Y ����ŭ
            {
                PlacedPos = new Vector3Int(x, y, 0);
				TileBase tempTile = tilemap.GetTile(PlacedPos);

                if(tempTile != null) //Ÿ���� ��ġ�Ǿ� �ִٸ�
                {
                    TempContainer.PlacedTiles.Add(tempTile); //�ش� Ÿ���� �����Ϳ� ����
                    TempContainer.PlacedPoses.Add(PlacedPos); //�ش� Ÿ�� ��ǥ�� �����Ϳ� ����
                }
            }
        }

        return TempContainer; //��ġ Ÿ�� ������ ��ȯ
    }

	#endregion

}