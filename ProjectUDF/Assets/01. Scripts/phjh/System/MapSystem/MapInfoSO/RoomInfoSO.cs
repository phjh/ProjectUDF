using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfo", menuName = "SO/Map/RoomsInfo")]
public class RoomInfoSO : ScriptableObject
{
    [Header("�� ������")]
    public int id;
    public int monsterWaves;
    public GameObject MapPrefab;

    [Header("���̺꺰 ������ ���� ��")]
    public List<int> numberOfMonsters;

    [Header("���� ����Ʈ")]
    public List<GameObject> spawnMonsterList; //��ȯ ���ɼ��ִ� ���͵�
    //[HideInInspector]
    //�ǵ�� �ȵ�
    public List<GameObject> spawnMonster; //��ȯ�� ���͵�

    public RoomInfoSO CloneAndSetting()
    {
        var a = Instantiate(this);
        a.GenerateMonsterInfo();
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
                int rand = Random.Range(0, spawnMonsterList.Count);
                spawnMonster.Add(spawnMonsterList[rand]); 
            }
        }
    }

    public void GenerateMonsterInfo()
    {

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
                str += spawnMonster[i] + " | ";
                i++;
            }
            Debug.Log($"{str} __ {a}��° ���̺� ��");
        }
    }

}