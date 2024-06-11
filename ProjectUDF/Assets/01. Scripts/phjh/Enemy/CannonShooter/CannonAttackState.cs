using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cannon Attack EnemyMotionState", menuName = "SO/EnemyMotionState/Attack/Throw")]
public class CannonAttackState : EnemyState
{
    public float chargingTime = 1f;
    public float waitThrow = 1f;
    public float waitRange = 1f;
    public float waitFall = 1f;
    public float effectdur = 0.4f;
    
    private Animator animator;
    private Coroutine attackCoroutine;
    private GameObject attackRange;
    private SpriteRenderer attackSRangeSp;
    private Collider2D attackRangeCol;

    public override EnemyState Clone()
    {
        CannonAttackState clone = CloneBase() as CannonAttackState;
        clone.animator = enemy.GetComponentInChildren<Animator>();
        clone.attackRange = enemy.transform.Find("AttackRange").gameObject;
        clone.attackSRangeSp = clone.attackRange.GetComponent<SpriteRenderer>();
        clone.attackRangeCol = clone.attackRange.GetComponent<Collider2D>();
        clone.chargingTime = chargingTime;
        clone.waitThrow = waitThrow;
        clone.waitFall = waitFall;
        clone.effectdur = effectdur;
        return clone;
    }

    public override void EnterState()
    {
        base.EnterState();
        attackRange.SetActive(false);
        attackRangeCol.enabled = false;
        //Debug.Log("before start coroutine");
        attackCoroutine = enemy.StartCoroutine(AttackCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();
        if (attackCoroutine != null)
        {
            enemy.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        Debug.Log("Coroutine Started");
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.CannonCharge, enemy.transform.position + Vector3.up, chargingTime);
        yield return new WaitForSeconds(chargingTime);
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(waitThrow);
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.CannonFire, enemy.transform.position + Vector3.up * 2, effectdur);
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.CannonRebound, enemy.transform.position + Vector3.up / 2, effectdur);
        animator.SetBool("isShooting", false);
        yield return new WaitForSeconds(waitRange);
        Vector2 attackPos = enemy.Target.position;
        attackRange.transform.position = attackPos;
        attackRange.SetActive(true);
        //¿Ã∆Â∆Æ √ﬂ∞° ∞ÌπŒ¡ﬂ
        attackSRangeSp.DOColor((new Color(255, 45, 45, 200) / 255), waitFall).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(waitFall);
        attackRangeCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackRangeCol.enabled = false;
        attackRange.SetActive(false);
        attackSRangeSp.color = (new Color(255, 45, 45, 140) / 255);
        //attackRangeSp.material = mat;
        attackCoroutine = null;
        enemy.StateMachine.ChangeState(enemy.CooldownState);
    }

}
