using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBaseAttack : PlayerBaseAttack
{

    protected override void Start()
    {
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

        PlayerMain.Instance.playerMove.SetFixedDir(true, PlayerMain.Instance.playerAim.Mousedir.normalized);

        //���ݹ��� ǥ��
        attackRange.gameObject.SetActive(true);

        //������
        PlayerMain.Instance.canAttack = false;

        //������ ���ϱ�
        float damage = CalculateDamage(damageFactor);
        Debug.Log("damage : " + damage);

        //����Ʈ ���
        //EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 3, 0.4f);

        //������ ����
        PlayerMain.Instance.canMove = false;

        atkcollider.enabled = true;
    }

    protected override void OnAttackStart()
    {
        base.OnAttackStart();
        PlayerMain.Instance.isAttacking = true;
    }

    protected override void OnAttacking()
    {      
        Debug.Log("onattacking");
        attackRange.gameObject.SetActive(false);
        atkcollider.enabled = false;
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

    protected override void StrengthStoneAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void LuckyStoneAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackSpeedStoneAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void MoveSpeedStoneAttack()
    {
        throw new System.NotImplementedException();
    }
}
