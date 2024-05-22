using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonsterInfo 
{
    public GameObject monsterObj;
    public Vector2 monsterPos;
}

[Serializable]
public enum Exit
{
    Down=0,
    Right,
    Up,
    Left
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

    [Tooltip("�ⱸ ��ġ��")]
    public List<Exit> exit;

    public RoomInfoSO CloneAndSetting()
    {
        var thisMap = Instantiate(this);
        Debug.Log(thisMap);
        return thisMap;
    }

    public RoomInfoSO CloneAndSettingRandom()
    {
        var thisMap = Instantiate(this);
		thisMap.GenerateRandomMonsterInfo();
        //a.SetExitPoint();
        if (exit.Count == 0)
        {
            Debug.LogError("exit room is not exist");
        }
        return thisMap;
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
            Debug.Log($"{str} __ {a}��° ���̺� ��");
        }
    }

    //public void SetExitPoint()
    //{
    //    int rand = UnityEngine.Random.Range(0, 4);
    //    exit = (Exit)rand;
    //}

}