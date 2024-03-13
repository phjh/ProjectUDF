using System;
using UnityEngine;

public enum Stats
{
    Strength = 0,
    Lucky = 1,
    MoveSpeed = 2,
    AttackSpeed = 3,
    HP = 4
}

[CreateAssetMenu(fileName = "New Player Stat", menuName = "SO/Player/PlayerStat")]
public class PlayerStat : ScriptableObject
{
    public event Action<float> StrengthChanged;
    public event Action<float> HpChanged;
    public event Action<float> MoveSpeedChanged;
    public event Action<float> AttackSpeedChanged;
    public event Action<float> LuckyChanged;

    public float PlayerStrength;
    public float PlayerHP;
    public float PlayerMoveSpeed;
    public float PlayerAttackSpeed;
    public float PlayerLucky;

    public void OnEnable()
    {
        SetStatStart();
        Debug.Log("���� �� �ʱ�ȭ ����");
    }

    public PlayerStat Clone()
    {
        var returnvalue = Instantiate(this);
        return returnvalue;
    }

    //�⺻������ �߰������̴� �����Ұ�
    public void EditStat(Stats statName, float EditingAmount)
    {
        switch (statName)
        {
            case Stats.Strength:
                PlayerStrength += EditingAmount;
                StrengthChanged?.Invoke(PlayerStrength);
                break;
            case Stats.HP:
                PlayerHP += EditingAmount;
                HpChanged?.Invoke(PlayerHP);
                break;
            case Stats.MoveSpeed:
                PlayerMoveSpeed += EditingAmount;
                MoveSpeedChanged?.Invoke(PlayerMoveSpeed);
                break;
            case Stats.AttackSpeed:
                PlayerAttackSpeed += EditingAmount;
                AttackSpeedChanged?.Invoke(PlayerAttackSpeed);
                break;
            case Stats.Lucky:
                PlayerLucky += EditingAmount;
                LuckyChanged?.Invoke(PlayerLucky);
                break;
        }

        Debug.Log($"[���� ����] | �� : {PlayerStrength} | �̼� : {PlayerMoveSpeed} | ���� : {PlayerAttackSpeed} | �� : {PlayerLucky} | ü�� : {PlayerHP} |");
    }

    public void SetStatStart()
    {
        PlayerStrength = 4;
        PlayerHP = 10;
        PlayerMoveSpeed = 5;
        PlayerAttackSpeed = 1;
        PlayerLucky = 99;
    }


}
