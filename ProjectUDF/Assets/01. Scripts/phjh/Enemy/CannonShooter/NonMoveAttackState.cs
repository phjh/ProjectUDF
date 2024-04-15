using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cannon Attack State", menuName = "SO/State/Attack/Throw")]
public class NonMoveAttackState : EnemyState
{
    public float detectTime = 0.4f;
    public float thrownObjTime = 1f;
    public Vector3 maxScale = Vector3.one + Vector3.one/2;

    private Coroutine attackCoroutine;

    public override EnemyState Clone()
    {
        NonMoveAttackState clone = CloneBase() as NonMoveAttackState;
        clone.detectTime = detectTime;
        clone.thrownObjTime = thrownObjTime;
        clone.maxScale = maxScale;
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
        yield return new WaitForSeconds(detectTime);
        Vector2 attackDir = enemy.Target.position;
        var throwObj = PoolManager.Instance.Pop(PoolObjectListEnum.ToadRock);
        throwObj.transform.position = enemy.transform.position;
        throwObj.transform.localScale = new Vector2(1.3f, 1.3f);
        Debug.Log("befor seq");
        Sequence seq = DOTween.Sequence();
        yield return seq.Append(throwObj.transform.DOMove(attackDir, thrownObjTime))
                        .Join(throwObj.transform.DOScale(maxScale, thrownObjTime * 0.4f)).SetEase(Ease.OutQuad)
                        .Insert(0.4f, throwObj.transform.DOScale(Vector3.one, thrownObjTime * 0.55f)).SetEase(Ease.InQuad);
        Debug.Log("after seq");
        yield return new WaitForSeconds(thrownObjTime);
        PoolManager.Instance.Push(throwObj, PoolObjectListEnum.ToadRock);
        attackCoroutine = null;
        enemy.StateMachine.ChangeState(enemy.CooldownState);
    }

}
