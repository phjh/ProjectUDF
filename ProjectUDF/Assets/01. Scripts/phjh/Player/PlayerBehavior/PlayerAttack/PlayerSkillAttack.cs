using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillAttack : PlayerAttack
{
    //ÄðÅ¸ÀÓ °ü·Ã
    [SerializeField]
    protected int _coolattackTime;
    protected int _coolattackTimeSet;

    protected override void OnEnable()
    {
        PlayerMain.Instance.OnAttackEvent += ReduceCooltime;
    }

    protected override void OnDisable()
    {
        //PlayerMain.Instance.OnAttackEvent -= ReduceCooltime;    
    }

    protected override void TryAttack()
    {
        if(_coolattackTime <= 0)
        {
            _coolattackTime = _coolattackTimeSet;
            Debug.Log("skill Invoked");
        }
        else
        {
            Debug.Log($"cooltime (leftCooltime : {_coolattackTime - (_coolattackTimeSet)}");
            return;
        }
    }

    private void ReduceCooltime() => _coolattackTime--;

}
