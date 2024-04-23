using System;
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
    public PlayerBehaviour behaviour { get; private set; }
    public InputReader inputReader { get; private set; }




    #region ������ ���� ������

    public bool canMove { get; set; }

    private Rigidbody2D rb;

    #endregion

    #region ���� ���� ������

    public Action OnAttackEvent;

    public PlayerAim playerAim { get; private set; }

    public bool canAttack { get; set; }
    public bool isAttacking { get; set; }

    public float recentDamage { get; set; }


    #endregion

    public void Init(PlayerStat stat)
    {
        playerAim = GetComponent<PlayerAim>();
        this.stat = stat;
    }

    private void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
    }


    public void Attack(PlayerBAttack attack)
    {
        attack.Attack(attack);
    }


}
