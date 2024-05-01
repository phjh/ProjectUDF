using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    //데미지 관련
    protected float _damageFactor { get; set; }
    protected bool _isCritical = false;

    private float recentDamage;

    private float baseCriticalChance = 5;

    private float LuckyStatValue;
    private float StrengthStatValue;

    protected virtual void OnEnable()
    {
        PlayerStats.OnStatChanged += UpdateStat;
        UpdateStat();
    }

    protected virtual void OnDisable()
    {

    }

    public void Attack(PlayerAttack attack)
    {
        attack.TryAttack();
    }


    protected abstract void TryAttack();

    private void UpdateStat()
    {
        LuckyStatValue = PlayerMain.Instance.stat.Lucky.GetValue();
        StrengthStatValue = PlayerMain.Instance.stat.Strength.GetValue();
    }

    protected float CalculateDamage(float factor)
    {
        GetRecentDamage();
        float damage = 0;
        bool critical = false;
        if (UnityEngine.Random.Range(0, 100.0f) < PlayerMain.Instance.stat.Lucky.GetValue() + baseCriticalChance)
        {
            damage = recentDamage * 1.3f * factor;
            critical = true;
        }
        else
        {
            damage = PlayerMain.Instance.stat.Strength.GetValue() * factor;
        }
        recentDamage = Mathf.Ceil(damage * 10) / 10;
        _isCritical = critical;
        PlayerMain.Instance.isCritical = critical;
        SetRecentDamage();
        return damage;
    }

    private void SetRecentDamage() => PlayerMain.Instance.recentDamage = recentDamage;

    private void GetRecentDamage() => recentDamage = PlayerMain.Instance.recentDamage;

}
