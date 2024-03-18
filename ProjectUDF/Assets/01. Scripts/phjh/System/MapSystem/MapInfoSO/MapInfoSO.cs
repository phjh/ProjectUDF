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

[CreateAssetMenu(fileName = "RandomMapInfoSO", menuName = "SO/Map/RandomMapInfo")]
public class MapInfoSO : ScriptableObject
{
    public int monsterWaves;
    //waveMonsters[nowWave][spawn enemies]
    public List<List<GameObject>> waveMonsters;
    public List<int> numberOfMonster;
    public List<GameObject> spawnMonsterList;
    //광물들 
    public List<GameObject> Ores;
    
    public MapInfoSO CloneAndSetting()
    {
        var clone = Instantiate(this);
        clone.GenerateRandomMapInfoSO();
        return clone;
    }

    public async void GenerateRandomMapInfoSO()
    {
        Debug.Log("Start Map Info Generating");
        await Task.Run(()=>{
            for(int nowwave = 0;nowwave< monsterWaves; nowwave++) //웨이브 수 만큼 반복
            {
                for(int monsters = 0; monsters < numberOfMonster.Count; monsters++)
                {
                    int rand = Random.Range(0,spawnMonsterList.Count);
                    waveMonsters[nowwave].Add(spawnMonsterList[rand]);
                }
                for(int i = 0; i < 3; i++ )
                {
                    int rand = Random.Range(0, (int)OreList.End);
                    Ores.Add(Ores[rand]);
                }
            }
        });
        Debug.Log("Susscessful Map Info Generated!");
    }



}
