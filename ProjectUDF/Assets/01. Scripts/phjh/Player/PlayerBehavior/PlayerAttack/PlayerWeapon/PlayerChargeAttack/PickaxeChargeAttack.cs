using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PickaxeChargeAttack : PlayerChargeAttack, IStopAttractable
{

    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        _showRange = true;
        attackRange.SetActive(true);
        charged += Time.deltaTime;
        float scale = GetChargedFactor(charged);
        attackRange.transform.DOScale(new Vector3(scale, scale, scale), 0.2f);
        //float scale = Mathf.Lerp(1.4f, 1.8f, Mathf.Clamp(charged / 1, 0, 1));
        //attackRange.transform.localScale = new Vector3(scale, scale, scale);
    }

    protected override void OnAttackStart()
    {
        if (!_showRange)
            return;

        InvokeStoneAttack();

        float baseFactor = _damageFactor;
        _damageFactor = GetChargedFactor(charged);

        StopAiming();

        //������
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //������ ���ϱ�
        float damage = CalculateDamage(_damageFactor);
        Debug.Log("damage : " + damage);

        atkcollider.enabled = true;

        _damageFactor = baseFactor;

        //���� �Լ� ����
        Invoke(nameof(OnAttacking), timeToAttacking);
    }

    protected override void OnAttacking()
    {
        //���� ���ۿ��� ���� ó�� ���õ� �͵� �����ϱ�
        StartAiming();

        Debug.Log("onattacking");
        PlayerMain.Instance.canMove = true;
        atkcollider.enabled = false;
        attackRange.gameObject.SetActive(false);

        //���� �Լ� ����
        Invoke(nameof(OnAttackEnd), timeToEnd);
    }

    protected override void OnAttackEnd()
    {
        charged = 0f;
        attackRange.transform.localScale = Vector3.one;
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
    }

    public void StartAiming()
    {
        PlayerMain.Instance.playerAim.enabled = true;
    }

    public void StopAiming()
    {
        PlayerMain.Instance.playerAim.enabled = false;
    }

    protected override void StrengthStoneAttack()
    {
        int step = HowlongCharged(charged) + 1;

        //����Ʈ ������ش�
        PoolableMono poolitem = EffectSystem.Instance.BasePop(PoolEffectListEnum.CannonFire, transform.position, 1 + step / 6);
        poolitem.GetComponent<ParticleSystem>().Play();

        Vector3 dir = PlayerMain.Instance.playerAim.Mousedir.normalized * step * 0.9f;

        poolitem.transform.DOMove(transform.position + dir, 0.5f + step / 6);

    }


    protected override void LuckyStoneAttack()
    {

    }

    protected override void AttackSpeedStoneAttack()
    {
        Debug.Log("�������� ���Ӱ����� �����");
    }

    protected override void MoveSpeedStoneAttack()
    {
        Debug.Log("�̼� �� ������ �̺�Ʈ");
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
        while(time < 0.5f)
        {
            PlayerMain.Instance.playerMove.SetFixedDir(true, PlayerMain.Instance.playerAim.Mousedir.normalized * step * curve.Evaluate(time * 2) * 4);
            yield return new WaitForSeconds(0.02f);
            time += 0.02f;

        }
        PlayerMain.Instance.playerMove.SetFixedDir(false, Vector2.zero);
    }

}
