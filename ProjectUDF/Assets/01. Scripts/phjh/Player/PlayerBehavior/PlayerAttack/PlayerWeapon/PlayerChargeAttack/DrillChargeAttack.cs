using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DrillChargeAttack : PlayerChargeAttack
{
    [SerializeField]
    AnimationReferenceAsset dig_Animation;
    [SerializeField]
    AnimationReferenceAsset digUp_Animation;

    float time = 0;

    protected override void Start()
    {
        base.Start();
    }

    void SetAnimation()
    {
        while(time <= dig_Animation.Animation.Duration)
        {
            SpineAnimator.Instance.SetEmptyAnimation(skele_Animator, 0, 0.1f);
            SpineAnimator.Instance.SetEmptyAnimation(skele_Animator, 1, 0.1f);
            SpineAnimator.Instance.SetAnimation(skele_Animator, dig_Animation, 0, startTime:time - dig_Animation.Animation.Duration);
            SpineAnimator.Instance.SetAnimation(skele_Animator, dig_Animation, 1, startTime:time - dig_Animation.Animation.Duration);
            time += Time.deltaTime;
        }
        SpineAnimator.Instance.SetEmptyAnimation(skele_Animator, 1, 1000);
        SpineAnimator.Instance.AddAnimation(skele_Animator, AttackingAnimation[0], dig_Animation.Animation.Duration, 0, true);
        
    }

    public override void OnAttackPrepare()
    {
        Debug.Log("preparing");
        if (!CanAttack())
            return;

        _showRange = true;
        attackRange.SetActive(true);
        PlayerMain.Instance.preparingAttack = true;
        charged += Time.deltaTime;
        PlayerMain.Instance.isInvincible = true;
        if(time <= dig_Animation.Animation.Duration + 0.1)
            SetAnimation();
    }

    protected override void OnAttackStart()
    {
        Debug.Log("atkStart");
        if (!_showRange)
            return;

        InvokeStoneAttack();

        float baseFactor = _damageFactor;
        _damageFactor = GetChargedFactor(charged);

        //공격중
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //데미지 구하기
        float damage = CalculateDamage(_damageFactor);
        Debug.Log("damage : " + damage);

        atkcollider.enabled = true;

        _damageFactor = baseFactor;

        SpineAnimator.Instance.SetAnimation(skele_Animator, digUp_Animation, 0);

        Invoke(nameof(OnAttacking), timeToAttacking);
    }

    protected override void OnAttacking()
    {

        Debug.Log("onattacking");
        PlayerMain.Instance.canMove = true;
        atkcollider.enabled = false;
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.isInvincible = false;
        PlayerMain.Instance.preparingAttack = false;
        SpineAnimator.Instance.SetEmptyAnimation(skele_Animator, 1, 0.1f);

        Invoke(nameof(OnAttackEnd), timeToEnd);
    }

    protected override void OnAttackEnd()
    {
        charged = 0f;
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
        time = 0;
        Debug.Log("chargeAttackend");
    }

    protected override void StrengthStoneAttack()
    {
        int step = HowlongCharged(charged) + 1;

    }

    protected override void LuckyStoneAttack()
    {

    }

    protected override void AttackSpeedStoneAttack()
    {
        Debug.Log("공속 차징엔 읎다");
    }

    protected override void MoveSpeedStoneAttack()
    {
        Debug.Log("이속 돌 강공격 이벤트");
        //int step = HowlongCharged(charged) + 1;
        //Vector3 targetPos = PlayerMain.Instance.playerAim.Mousedir.normalized * step * 2;
        //PlayerMain.Instance.playerMove.Dash(0.5f, transform.position + targetPos);

        StartCoroutine(MoveSpeedStoneDash());
    }

    public AnimationCurve curve;
    IEnumerator MoveSpeedStoneDash()
    {
        int step = HowlongCharged(charged) + 1;
        float time = 0;
        while (time < step / 1.5f) 
        {
            PlayerMain.Instance.playerMove.SetFixedDir(true, PlayerMain.Instance.playerAim.Mousedir.normalized * step * curve.Evaluate(time * 2) * 4);
            yield return new WaitForSeconds(0.02f);
            time += 0.02f;

        }
        PlayerMain.Instance.playerMove.SetFixedDir(false, Vector2.zero);
    }
}
