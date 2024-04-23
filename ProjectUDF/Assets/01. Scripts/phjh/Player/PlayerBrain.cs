using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//여기선 behavior을 통해 세팅과 흐름제어만 할 예정이다
public class PlayerBrain : MonoBehaviour
{
    public PlayerStat stat;
    public PlayerBehaviour behaviour;
    
    public WeaponInfo nowWeapon;
    public PlayerSkillAttack nowSkill;

    public GameObject WeaponParent; //디버깅용

    private void Awake()
    {
        PlayerMain.Instance.Init(stat);
    }

    public void Start()
    {
        WeaponChange(nowWeapon);
        SkillChange(nowSkill);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) 
        {
            PlayerBaseAttack baseatk = WeaponParent.GetComponentInChildren<PlayerBaseAttack>();
            PlayerChargeAttack chargeatk = WeaponParent.GetComponentInChildren<PlayerChargeAttack>();
            WeaponInfo info = new WeaponInfo();
            info.baseAttack = baseatk;
            info.chargeAttack = chargeatk;
            WeaponChange(info);
        }
        if(Input.GetMouseButtonDown(0))
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
