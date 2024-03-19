using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")]
public class RoomInfoSO : ScriptableObject
{
    [Header("�� ������")]
    public int id;
    public int monsterWaves;
    public List<int> numberOfMonsters;

    [Header("���� ����Ʈ")]
    public List<GameObject> spawnMonsterList;
    [HideInInspector]
    public List<GameObject> spawnMonster;
    //�ǵ�� �ȵ�

    public RoomInfoSO CloneAndSetting()
    {
        var a = Instantiate(this);
        a.GenerateRandomMonsterSpawnInfo();
        return a;
    }

    private void GenerateRandomMonsterSpawnInfo()
    {
        if (numberOfMonsters.Count != monsterWaves)
        {
            Debug.LogError("�� Ʋ�� : monsterWaves");
            return;
        }
        for (int i = 0; i < monsterWaves; i++)
        {
            int a = numberOfMonsters[i];
            for (int j = 0; j < a; j++)
            {
                int rand = Random.Range(0, spawnMonsterList.Count);
                spawnMonster.Add(spawnMonsterList[rand]); 
            }
        }
    }

}