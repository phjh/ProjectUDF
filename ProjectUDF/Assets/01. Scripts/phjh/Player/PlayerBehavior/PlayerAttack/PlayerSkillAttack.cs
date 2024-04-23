using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillAttack : PlayerBAttack
{
    //ÄðÅ¸ÀÓ °ü·Ã
    [SerializeField]
    protected int _cooltimes;
    protected int _cooltimeSet;


    protected override void OnEnable()
    {
        PlayerMain.Instance.OnAttackEvent += ReduceCooltime;
    }

    protected override void OnDisable()
    {
        PlayerMain.Instance.OnAttackEvent -= ReduceCooltime;    
    }

    protected override void TryAttack()
    {
        if(_cooltimes <= 0)
        {
            _cooltimes = _cooltimeSet;
            Debug.Log("skill Invoked");
        }
        else
        {
            Debug.Log($"cooltime (leftCooltime : {_cooltimes - (_cooltimeSet)}");
            return;
        }
    }

    private void ReduceCooltime() => _cooltimes--;

}
