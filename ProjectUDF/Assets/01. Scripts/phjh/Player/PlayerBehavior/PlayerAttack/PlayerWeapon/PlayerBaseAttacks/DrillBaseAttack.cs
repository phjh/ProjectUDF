using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBaseAttack : PlayerBaseAttack
{
    PlayerMovement move;

    private void Start()
    {
        move = GetComponentInParent<PlayerMovement>();
    }

    public override void OnAttackPrepare()
    {
        if (move == null)
            Debug.LogError("move is null");

        if (PlayerMain.Instance.isDodging)
        {
            Debug.Log("Dodging");
            return;
        }

        move.SetFixedDir(true, PlayerMain.Instance.playerAim.Mousedir.normalized);

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
        move.SetFixedDir(false, Vector2.zero);

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
}
