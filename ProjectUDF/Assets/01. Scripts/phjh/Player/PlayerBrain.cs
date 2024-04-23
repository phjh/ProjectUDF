using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//여기선 behavior을 통해 세팅과 흐름제어만 할 예정이다
public class PlayerBrain : MonoBehaviour
{
    public PlayerStat stat;
    public InputReader reader;
    public PlayerAim aim;
    
    public WeaponInfo nowWeapon;
    public PlayerSkillAttack nowSkill;

    private void Awake()
    {
        PlayerMain.Instance.Init(aim, stat, reader);
    }

    public void Start()
    {
        WeaponChange(nowWeapon);
        SkillChange(nowSkill);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            nowWeapon.baseAttack.OnAttackPrepare();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            nowWeapon.baseAttack.Attack(nowWeapon.baseAttack);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            nowWeapon.chargeAttack.Attack(nowWeapon.chargeAttack);
        }

    }

    public void WeaponChange(WeaponInfo weapon)
    {
        nowWeapon = weapon;
    }

    public void SkillChange(PlayerSkillAttack skill)
    {
        nowSkill = skill;
    }

}
