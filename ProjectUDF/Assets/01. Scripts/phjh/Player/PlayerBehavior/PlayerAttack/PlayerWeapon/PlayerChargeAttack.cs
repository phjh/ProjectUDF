using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PlayerChargeAttack : PlayerWeaponAttack
{
    [Serializable]
    public struct ChargingInfo
    {
        public float time;
        public float factor;
    }

    [SerializeField]
    protected GameObject attackRange;
    [SerializeField]
    protected Collider2D atkcollider;

    protected float charged = 0;

    //[SerializeField]
    //[Tooltip("��ǥġ �ð� (ó���� 0�ʷ�)")]
    //protected List<float> ChargePartTime;
    //[SerializeField]
    //[Tooltip("�ð��� ���� ��� (ó���� �⺻ �����)")]
    //protected List<float> ChargePartFactor;

    [SerializeField]
    protected List<ChargingInfo> ChargeInfo;

    protected override void TryAttack()
    {
        if (!CanAttack())
            return;

        base.TryAttack();
        Debug.Log("attack Invoked");
        OnAttackStart();
    }

    protected float GetChargedFactor(float time)
    {
        for (int i = 0; i < ChargeInfo.Count; i++)
        {
            if (ChargeInfo[i].time > time)
                return ChargeInfo[i - 1].factor;
        }
        return ChargeInfo[ChargeInfo.Count - 1].factor;
    }

    //protected float GetChargedFactor(float time)
    //{
    //    for (int i = 0; i < ChargePartTime.Count; i++)
    //    {
    //        if (ChargePartTime[i] > time)
    //            return ChargePartFactor[i-1];
    //    }
    //    return ChargePartFactor[ChargePartFactor.Count - 1];
    //}
}
