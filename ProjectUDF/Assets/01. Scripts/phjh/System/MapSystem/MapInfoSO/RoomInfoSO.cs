using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MapDefine;

[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")] 
public class RoomInfoSO : ScriptableObject
{
    [Header("방 정보들")]
    public int id;
    public GameObject TileObject;
    [Range(30, 600)]public float timeLimit = 120;

    [Header("Monster Data")]
    public List<WaveData> RoomWaveData;

    [Tooltip("출구 위치")]
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

	private void SetTileMapComponent() //타일맵 컴포넌트를 받고, 각 타일 데이터를 세팅하는 함수를 작동시키는 곳
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
			SaveRoomData(); //타일 데이터 세팅 함수
		}
        else
        {
            Debug.Assert(ObstacleMap != null, "ObstacleMap Is Null");
            Debug.Assert(DecorateMap != null, "DecorateMap Is Null");
        }
	}

	private void SaveRoomData()
    {
        Obstacle = SavePlacedTIleData(ObstacleMap); //데이터 반환 할당
        Decorate = SavePlacedTIleData(DecorateMap); //데이터 반환 할당
    }

    private PlacedTileData SavePlacedTIleData(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int PlacedPos = new Vector3Int();

        PlacedTileData TempContainer = new PlacedTileData(); //배치 타일 데이터
        for(int x = bounds.min.x; x < bounds.max.x; x++) //바운드의 X 값만큼
        {
            for(int y = bounds.min.y; y < bounds.max.y; y++) //바운드의 Y 값만큼
            {
                PlacedPos = new Vector3Int(x, y, 0);
				TileBase tempTile = tilemap.GetTile(PlacedPos);

                if(tempTile != null) //타일이 배치되어 있다면
                {
                    TempContainer.PlacedTiles.Add(tempTile); //해당 타일을 데이터에 넣음
                    TempContainer.PlacedPoses.Add(PlacedPos); //해당 타일 좌표를 데이터에 넣음
                }
            }
        }

        return TempContainer; //배치 타일 데이터 반환
    }

	#endregion

}