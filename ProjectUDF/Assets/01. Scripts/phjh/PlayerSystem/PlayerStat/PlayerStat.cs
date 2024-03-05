using System;
using UnityEngine;

public enum Stats
{
    Strength,
    HP,
    MoveSpeed,
    AttackSpeed
}


[CreateAssetMenu(fileName = "New Player Stat", menuName = "SO/Player/PlayerStat")]
public class PlayerStat : ScriptableObject
{
    public event Action<float> StrengthChanged;
    public event Action<float> HpChanged;
    public event Action<float> MoveSpeedChanged;
    public event Action<float> AttackSpeedChanged;

    public readonly float BaseStrength;
    public readonly float BaseHP;
    public readonly float BaseMoveSpeed;
    public readonly float BaseAttackSpeed;

    public float PlayerStrength;
    public float PlayerHP;
    public float PlayerMoveSpeed;
    public float PlayerAttackSpeed;

    public void OnEnable()
    {
        SetStatStart();
    }

    public PlayerStat Clone()
    {
        return Instantiate(this);
    }

    public void SetStatStart()
    {
        PlayerStrength = 4;
        PlayerHP = 10;
        PlayerMoveSpeed = 5;
        PlayerAttackSpeed = 1;
    }

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
        }
    }



}
