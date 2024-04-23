using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//���⼱ behavior�� ���� ���ð� �帧��� �� �����̴�
public class PlayerBrain : MonoBehaviour
{
    public PlayerStat stat;
    public PlayerBehaviour behaviour;
    
    public WeaponInfo nowWeapon;
    public PlayerSkillAttack nowSkill;

    public GameObject WeaponParent; //������

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
