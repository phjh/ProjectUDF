using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public abstract class PlayerWeaponAttack : PlayerAttack
{
    public List<Action> AdditionalAttack = new();

    [SerializeField]
    protected List<AnimationReferenceAsset> AttackPrepareAnimation;

    [SerializeField]
    protected List<AnimationReferenceAsset> AttackingAnimation;

    [SerializeField]
    protected float timeToAttacking;
    [SerializeField]
    protected float timeToEnd;

    // 현재광석 선언, Dcttionary / 리스트 작성
    protected bool stoneActived;
    public bool isActiveonce;

    protected bool _showRange;

    protected virtual void Start()
    {
        Init();
    }

    protected void InvokeStoneAttack()
    {
        if (stoneActived)
            return;
        if (isActiveonce)
            return;
        AdditionalAttack[PlayerMain.Instance.EquipMainOre].Invoke();
    }

    protected void Init()
    {
        AdditionalAttack.Add(() => { });
        AdditionalAttack.Add(StrengthStoneAttack);
        AdditionalAttack.Add(LuckyStoneAttack);
        AdditionalAttack.Add(AttackSpeedStoneAttack);
        AdditionalAttack.Add(MoveSpeedStoneAttack);
    }

    protected virtual bool CanAttack()
    {
        if (PlayerMain.Instance.isAttacking)
        {
            Debug.Log("cooltime");
            return false;
        }
        return true;
    }
    protected override void TryAttack()
    {
        PlayerMain.Instance.OnAttackEvent?.Invoke();
    }

    public virtual void OnAttackPrepare() { }     //공격준비/차징 및 공격 범위를 보여준다
    protected abstract void OnAttackStart();    //직접적인 공격을 처리해준다
    protected abstract void OnAttacking();      //공격 후의 뒷처리를 해준다
    protected abstract void OnAttackEnd();      //공격 쿨타임 같은 마지막 작업을 해준다


    protected abstract void StrengthStoneAttack();
    protected abstract void LuckyStoneAttack();
    protected abstract void AttackSpeedStoneAttack();
    protected abstract void MoveSpeedStoneAttack();

    public virtual List<Stats> GetLuckyStones()
    {
        return new List<Stats>();
    }
    
    public virtual void SetLuckyStones(List<Stats> statList)
    {

    }

}
