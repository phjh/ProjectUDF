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
    [SerializeField] private GameObject TileObject;
    [Range(30, 600)]public float timeLimit = 120;

    [Header("Monster Data")]
    [Range(1, 5)]public int monsterWaves;
    public List<int> numberOfMonsters;
    public List<GameObject> spawnMonsterList; //��ȯ ���ɼ��ִ� ���͵�
    public List<MonsterInfo> spawnMonsters; //��ȯ�� ���͵�

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
        if (numberOfMonsters.Count != monsterWaves)
        {
            Debug.LogError("�� Ʋ�� : monsterWaves");
            return;
        }
        for (int waveCount = 0; waveCount < monsterWaves; waveCount++)
        {
            for (int monsterCount = 0; monsterCount < numberOfMonsters[waveCount]; monsterCount++)
            {
                int rand = UnityEngine.Random.Range(0, spawnMonsterList.Count);
                MonsterInfo monster;
                monster.monsterObj = spawnMonsterList[rand];
                monster.monsterPos = new Vector2(UnityEngine.Random.Range(-11, 11), UnityEngine.Random.Range(-6, 3));
                spawnMonsters.Add(monster);
            }
        }
    }

    /*public void DebugMonsters()
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
            Debug.Log($"{str} __ {a}��° ���̺� ��");
        }
    }*/

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