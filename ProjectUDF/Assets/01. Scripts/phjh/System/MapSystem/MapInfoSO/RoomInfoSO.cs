using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonsterInfo 
{
    public GameObject monsterObj;
    public Vector2 monsterPos;
}


[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")]
public class RoomInfoSO : ScriptableObject
{
    [Header("�� ������")]
    public int id;
    public int monsterWaves;
    public GameObject MapPrefab;
    public float timeLimit = 100;

    [Header("���̺꺰 ������ ���� ��")]
    public List<int> numberOfMonsters;

    [Header("���� ����Ʈ")]
    public List<GameObject> spawnMonsterList; //��ȯ ���ɼ��ִ� ���͵�
    //[HideInInspector]
    //�ǵ�� �ȵ�
    public List<MonsterInfo> spawnMonsters; //��ȯ�� ���͵�
    public RoomInfoSO CloneAndSetting()
    {
        var a = Instantiate(this);
        Debug.Log(a);
        return a;
    }

    public RoomInfoSO CloneAndSettingRandom()
    {
        var a = Instantiate(this);
        a.GenerateRandomMonsterInfo();
        Debug.Log(a);
        return a;
    }

    private void GenerateRandomMonsterInfo()
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
                int rand = UnityEngine.Random.Range(0, spawnMonsterList.Count);
                MonsterInfo monster;
                monster.monsterObj = spawnMonsterList[rand];
                monster.monsterPos = new Vector2(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10));
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
            Debug.Log($"{str} __ {a}��° ���̺� ��");
        }
    }

}