using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
        throw new NotImplementedException();
    }

    protected override void LuckyStoneAttack()
    {
        throw new NotImplementedException();
    }

    protected override void AttackSpeedStoneAttack()
    {
        Debug.Log("�������� ���Ӱ����� �����");
    }

    protected override void MoveSpeedStoneAttack()
    {
        Debug.Log("�̼� �� ������ �̺�Ʈ");
        StartCoroutine(MoveSpeedStoneDash());
    }


    IEnumerator MoveSpeedStoneDash()
    {
        int step = HowlongCharged(charged) + 1;
        PlayerMain.Instance.playerMove.SetFixedDir(true, PlayerMain.Instance.playerAim.Mousedir.normalized * step * 5);
        yield return new WaitForSeconds(0.5f);
        PlayerMain.Instance.playerMove.SetFixedDir(false, Vector2.zero);
    }

}
