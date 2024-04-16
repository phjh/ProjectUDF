using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cannon Attack State", menuName = "SO/State/Attack/Throw")]
public class CannonAttackState : EnemyState
{
    public float chargingTime = 1f;
    public float waitThrow = 1f;
    public float waitRange = 1f;
    public float waitFall = 1f;
    public float effectdur = 0.4f;
    
    private Animator animator;
    private Coroutine attackCoroutine;

    public override EnemyState Clone()
    {
        CannonAttackState clone = CloneBase() as CannonAttackState;
        clone.animator = enemy.GetComponentInChildren<Animator>();
        clone.chargingTime = chargingTime;
        clone.waitThrow = waitThrow;
        clone.waitFall = waitFall;
        clone.effectdur = effectdur;
        return clone;
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("before start coroutine");
        attackCoroutine = enemy.StartCoroutine(AttackCoroutine());
    }

    public override void ExitState()
    {
        //base.ExitState();
        //if (attackCoroutine != null)
        //{
        //    enemy.StopCoroutine(attackCoroutine);
        //    attackCoroutine = null;
        //}
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
        Vector2 attackDir = enemy.Target.position;
        //여기서 공격범위를 보여준다.

        yield return new WaitForSeconds(waitFall);

        //여기서 오브젝트가 덜어지는걸 생성한다.

        attackCoroutine = null;
        enemy.StateMachine.ChangeState(enemy.CooldownState);
    }

}
