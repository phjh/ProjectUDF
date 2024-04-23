using System;
using System.Runtime.Serialization;
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
    public InputReader inputReader { get; private set; }


    #region 움직임 관련 변수들

    public bool canMove { get; set; }

    public bool isDodging {  get; set; }

    private Rigidbody2D rb;

    #endregion

    #region 공격 관련 변수들

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
