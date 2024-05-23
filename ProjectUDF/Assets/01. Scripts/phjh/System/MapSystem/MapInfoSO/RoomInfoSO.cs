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
    [Range(1, 5)]public int monsterWaves;
    [SerializeField] private GameObject TileObject;
    [Range(30, 600)]public float timeLimit = 120;

    [Header("웨이브별 나오는 몬스터 수")]
    public List<int> numberOfMonsters;

    [Header("몬스터 리스트")]
    public List<GameObject> spawnMonsterList; //소환 가능성있는 몬스터들
    //[HideInInspector]
    //건들면 안됨
    public List<MonsterInfo> spawnMonsters; //소환될 몬스터들

    [Tooltip("출구 위치들")]
    public List<Exit> exits;

    [HideInInspector] public PlacedTileData Obstacle;
    [HideInInspector] public PlacedTileData Decorate;
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
                int rand = UnityEngine.Random.Range(0, spawnMonsterList.Count);
                MonsterInfo monster;
                monster.monsterObj = spawnMonsterList[rand];
                monster.monsterPos = new Vector2(UnityEngine.Random.Range(-11, 11), UnityEngine.Random.Range(-6, 3));
                spawnMonsters.Add(monster);
            }
        }
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
                str += spawnMonsters[i] + " | ";
                i++;
            }
            Debug.Log($"{str} __ {a}번째 웨이브 몹");
        }
    }

	#region Save Tilemap Data Method

	private void SetTileMapComponent()
	{
		if(ObstacleMap == null) ObstacleMap = TileObject.transform.Find("ObstacleTile").GetComponent<Tilemap>();
		if(DecorateMap == null) DecorateMap = TileObject.transform.Find("DecorateTile").GetComponent<Tilemap>();

		if (ObstacleMap != null && DecorateMap != null)
		{
			SaveRoomData();
		}
        else
        {
            Debug.Assert(ObstacleMap != null, "ObstacleMap Is Null");
            Debug.Assert(DecorateMap != null, "DecorateMap Is Null");
        }
	}

	private void SaveRoomData()
    {
        Obstacle = SavePlacedTIleData(ObstacleMap);
        Decorate = SavePlacedTIleData(DecorateMap);
    }

    private PlacedTileData SavePlacedTIleData(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int PlacedPos = new Vector3Int();

        PlacedTileData TempContainer = new PlacedTileData();
        for(int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for(int y = bounds.min.y; y < bounds.max.y; y++)
            {
                PlacedPos = new Vector3Int(x, y, 0);
				TileBase tempTile = tilemap.GetTile(PlacedPos);

                if(tempTile != null)
                {
                    TempContainer.PlacedTiles.Add(tempTile);
                    TempContainer.PlacedPoses.Add(PlacedPos);
                }
            }
        }

        return TempContainer;
    }

	#endregion

	//public void SetExitPoint()
	//{
	//    int rand = UnityEngine.Random.Range(0, 4);
	//    exit = (Exit)rand;
	//}

}