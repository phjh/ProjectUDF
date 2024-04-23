using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBAttack : MonoBehaviour
{
    //데미지 관련
    [SerializeField]
    protected float _damageFactor;
    protected bool _isCritical = false;

    private float recentDamage;

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {

    }

    public void Attack(PlayerBAttack attack)
    {
        attack.TryAttack();
    }


    protected abstract void TryAttack();

    protected float CalculateDamage()
    {
        GetRecentDamage();
        float damage = 0;
        bool critical = false;
        if (UnityEngine.Random.Range(0, 100.0f) < PlayerMain.Instance.stat.Lucky.GetValue())
        {
            damage = recentDamage * 1.3f * _damageFactor;
            critical = true;
        }
        else
        {
            damage = PlayerMain.Instance.stat.Strength.GetValue() * _damageFactor;
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
