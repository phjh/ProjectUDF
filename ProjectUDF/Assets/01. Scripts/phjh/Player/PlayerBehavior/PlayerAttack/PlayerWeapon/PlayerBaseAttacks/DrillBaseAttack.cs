using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBaseAttack : PlayerBaseAttack
{
    Queue<GameObject> hitCooldownObjs = new();

    [SerializeField]
    private float hitCooldown;

    public override void OnAttackPrepare()
    {        
        //공격범위 표시
        attackRange.gameObject.SetActive(true);

        //공격중
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //데미지 구하기
        float damage = CalculateDamage();
        Debug.Log("damage : " + damage);

        //이펙트 재생
        //EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 3, 0.4f);

        //움직임 제한
        PlayerMain.Instance.canMove = false;

        atkcollider.enabled = true;
    }

    protected override void OnAttackStart()
    {
        base.OnAttackStart();
    }

    protected override void OnAttacking()
    {      
        Debug.Log("onattacking");
        attackRange.gameObject.SetActive(false);
        atkcollider.enabled = false;

        base.OnAttacking();
    }

    protected override void OnAttackEnd()
    {
        PlayerMain.Instance.canMove = true;
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
        Debug.Log("onattackend");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy) && !hitCooldownObjs.Contains(collision.gameObject))
        {
            hitCooldownObjs.Enqueue(collision.gameObject);
            EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, PlayerMain.Instance.stat.Strength.GetValue(), PlayerMain.Instance.recentDamage, 0.5f, collision.transform.position, PlayerMain.Instance.isCritical);
            enemy.Damage(PlayerMain.Instance.recentDamage);
            GameManager.Instance.ShakeCamera();
            Invoke(nameof(SetCooldownObjList), hitCooldown);
        }
    }

    void SetCooldownObjList() => hitCooldownObjs.Dequeue();

}
