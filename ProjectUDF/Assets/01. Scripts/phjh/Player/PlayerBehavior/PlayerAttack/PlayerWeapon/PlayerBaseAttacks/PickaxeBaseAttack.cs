using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PickaxeBaseAttack : PlayerBaseAttack, IStopAttractable
{
    [SerializeField]
    GameObject attackRange;
    [SerializeField]
    Collider2D atkcollider;

    protected override void TryAttack()
    {
        if(!CanAttack())
            return;

        base.TryAttack();
        Debug.Log("attack Invoked");
        OnAttackStart();
    }

    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        //���� ���� ǥ��
        _showRange = true;
        attackRange.gameObject.SetActive(true);
    }

    protected override void OnAttackStart()
    {
        //������
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //������ ���ϱ�
        float damage = CalculateDamage();
        Debug.Log("damage : " + damage);

        //���ݹ��� ����
        StopAiming();

        //����Ʈ ���
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 2, 0.2f);
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.RightAttack, attackRange.transform.position + Vector3.up / 3, 0.4f);

        //������ ����
        PlayerMain.Instance.canMove = false;
        
        atkcollider.enabled = true;

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
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.isAttacking = false;
        PlayerMain.Instance.canAttack = true;
        Debug.Log("onattackend");
    }


    public void StartAiming()
    {
        PlayerMain.Instance.playerAim.enabled = true; 
    }

    public void StopAiming()
    {
        PlayerMain.Instance.playerAim.enabled = false;
    }
}
