using System;
using System.Runtime.Serialization;
using UnityEngine;

public class PlayerMain : MonoSingleton<PlayerMain>
{
    public PlayerStat stat { get; private set; }
    public InputReader inputReader { get; private set; }


    public Action OnAttackEvent;

    #region 움직임 관련 변수들

    public bool canMove { get; set; }

    public bool isDodging {  get; set; }

    private Rigidbody2D rb;

    #endregion

    #region 공격 관련 변수들


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


    public void Attack(PlayerAttack attack)
    {
        attack.Attack(attack);
    }


}
