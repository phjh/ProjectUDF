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
    
    public PlayerWeapon nowWeapon;

    private GameObject nowWeaponObj;
    private PlayerBaseAttack baseAttack;
    private PlayerChargeAttack chargeAttack;

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

        if(Input.GetMouseButton(0))
        {
            baseAttack.OnAttackPrepare();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            baseAttack.Attack(baseAttack);
        }
        if(Input.GetMouseButton(1))
        {
            chargeAttack.OnAttackPrepare();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            chargeAttack.Attack(chargeAttack);
        }
    }

    public void WeaponChange(PlayerWeapon weapon)
    {
        nowWeapon = weapon;
        nowWeaponObj = Instantiate(nowWeapon.baseAtk.gameObject, FindObjectOfType<PlayerAim>().transform);
        baseAttack = nowWeaponObj.GetComponent<PlayerBaseAttack>();
        chargeAttack = nowWeaponObj.GetComponent<PlayerChargeAttack>();

    }

    public void SkillChange(PlayerSkillAttack skill)
    {
        nowSkill = skill;
    }

}
