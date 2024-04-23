using System;
using UnityEngine;

[Serializable]
public struct WeaponInfo //여기서 평타랑 차징공격을 준비해준다
{
    public PlayerBaseAttack baseAttack; //평타
    public PlayerChargeAttack chargeAttack; //차징공격

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




    #region 움직임 관련 변수들

    public bool canMove { get; set; }

    private Rigidbody2D rb;

    #endregion

    #region 공격 관련 변수들

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
