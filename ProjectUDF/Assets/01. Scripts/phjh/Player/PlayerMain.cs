using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public struct WeaponInfo //���⼭ ��Ÿ�� ��¡������ �غ����ش�
{
    public PlayerBaseAttack baseAttack; //��Ÿ
    public PlayerChargeAttack chargeAttack; //��¡����

    public override string ToString()
    {
        return $"baseattack : {baseAttack}\n chargeattack : {chargeAttack}";
    }
}

public class PlayerMain : MonoSingleton<PlayerMain>
{
    public PlayerStat stat { get; private set; }
    public InputReader inputReader { get; private set; }


    #region ������ ���� ������

    public bool canMove { get; set; }

    public bool isDodging {  get; set; }

    private Rigidbody2D rb;

    #endregion

    #region ���� ���� ������

    public Action OnAttackEvent;

    public PlayerAim playerAim { get; private set; }

    public bool canAttack { get; set; }
    public bool isAttacking { get; set; }
    public float recentDamage { get; set; }
    public bool isCritical { get; set; }

    #endregion

    public void Init(PlayerAim aim, PlayerStat stat, InputReader reader)
    {
        playerAim = aim;
        this.stat = stat;
        inputReader = reader;
    }

    private void Awake()
    {
        canMove = true;
        isDodging = false;

        canAttack = true;
        isAttacking = false;
        recentDamage = 4f;
        isCritical = false;
    }


    public void Attack(PlayerBAttack attack)
    {
        attack.Attack(attack);
    }


}
