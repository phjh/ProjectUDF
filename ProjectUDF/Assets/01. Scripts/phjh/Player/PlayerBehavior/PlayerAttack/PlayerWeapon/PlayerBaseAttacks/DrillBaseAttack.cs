using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBaseAttack : PlayerBaseAttack
{ 
    private GameObject baseAttackRange;
    private Collider2D baseCollider;
    private float moveMultiply = 1;

    private bool exceptionAttack = false;

    protected override void Start()
    {
        baseAttackRange = attackRange;
        atkSpeedcol = AttackSpeedStoneObj.GetComponentInChildren<Collider2D>();
        base.Start();
    }

    public override void OnAttackPrepare()
    {
        if (PlayerMain.Instance.playerMove == null)
            Debug.LogError("move is null");

        if (PlayerMain.Instance.isDodging)
        {
            Debug.Log("Dodging");
            return;
        }

        InvokeStoneAttack();

        PlayerMain.Instance.preparingAttack = true;

        //공격범위 표시
        attackRange.gameObject.SetActive(true);

        //공격중
        PlayerMain.Instance.canAttack = false;

        //데미지 구하기
        float damage = CalculateDamage(damageFactor);
        Debug.Log("damage : " + damage);

        if (exceptionAttack)
        {

            return;
        }

        //이펙트 재생
        //EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 3, 0.4f);
        PlayerMain.Instance.playerMove.SetFixedDir(true, PlayerMain.Instance.playerAim.Mousedir.normalized * moveMultiply);

        //움직임 제한
        PlayerMain.Instance.canMove = false;

        atkcollider.enabled = true;
    }

    protected override void OnAttackStart()
    {
        base.OnAttackStart();
        PlayerMain.Instance.isAttacking = true;
        if(exceptionAttack)
            atkcollider.enabled = true;
    }

    protected override void OnAttacking()
    {      
        Debug.Log("onattacking");
        attackRange.gameObject.SetActive(false);
        atkcollider.enabled = false;
        PlayerMain.Instance.preparingAttack = false;
        PlayerMain.Instance.playerMove.SetFixedDir(false, Vector2.zero);

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

    [SerializeField]
    [Tooltip("힘의돌 쓸때 범위 증가 계수")]
    private float StrengthStoneMultiply;
    protected override void StrengthStoneAttack()
    {
        if (!isActiveonce)
        {
            attackRange.transform.localScale = attackRange.transform.localScale * StrengthStoneMultiply;
            moveMultiply = 1.5f;
            isActiveonce = true;
        }
        else
        {
            attackRange.transform.localScale = attackRange.transform.localScale / StrengthStoneMultiply;
            moveMultiply = 1;
            isActiveonce = false;
        }
    }

    protected override void LuckyStoneAttack()
    {

    }

    public GameObject AttackSpeedStoneObj;
    private Collider2D atkSpeedcol;
    protected override void AttackSpeedStoneAttack()
    {
        //플레이어 차징공격처럼 바꿔준다
        if (!isActiveonce)
        {
            exceptionAttack = true;
            attackRange = AttackSpeedStoneObj;
            atkcollider = atkSpeedcol;
            isActiveonce = true;
        }
        else
        {
            exceptionAttack = false;
            attackRange = baseAttackRange;
            atkcollider = baseCollider;
            isActiveonce = false;
        }
    }

    protected override void MoveSpeedStoneAttack()
    {
        //없음
        Debug.Log("드릴 이속광석 효과는 없어요");
    }
}
