using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBaseAttack : PlayerBaseAttack, IAttractable
{
    [SerializeField]
    GameObject attackRange;
    [SerializeField]
    Collider2D atkcollider;

    protected override void TryAttack()
    {
        if (PlayerMain.Instance.isAttacking)
            return;

        base.TryAttack();
        Debug.Log("attack Invoked");
        OnAttackStart();
    }

    public override void OnAttackPrepare()
    {
        //���� ���� ǥ��
        _showRange = true;
        attackRange.gameObject.SetActive(true);
    }

    protected override void OnAttackStart()
    {

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

        //���� �Լ� ����
        base.OnAttackStart();
    }

    protected override void OnAttacking()
    {
        //���� ���ۿ��� ���� ó�� ���õ� �͵� �����ϱ�
        StartAiming();

        PlayerMain.Instance.canMove = true;
        atkcollider.enabled = false;
        attackRange.gameObject.SetActive(false);

        //���� �Լ� ����
        base.OnAttacking();
    }

    protected override void OnAttackEnd()
    {
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
    }

    private IEnumerator NormalAttack()
    {
        Debug.Log("normal Attack!");
        yield return new WaitForSeconds(1f);
        Debug.Log("normal Attack Down!");
        //float damage = CalculateDamage(0.8f);
        //Debug.Log("damage : " + damage);
        //EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.RightAttack, _baseAttackRange.transform.position + Vector3.up / 3, 0.4f);
        //EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.MineCustom, _baseAttackRange.transform.position + Vector3.up / 2, 0.2f);
        //_aim.enabled = false;
        //PlayerMain.Instance.canMove = false;
        //_baseAttackCol.enabled = true;
        //yield return new WaitForSeconds(0.3f);
        //_aim.enabled = true;
        //PlayerMain.Instance.canMove = true;
        //_baseAttackCol.enabled = false;
        //_baseAttackRange.gameObject.SetActive(false);
        //yield return new WaitForSeconds(1.5f / (2 + (PlayerMain.Instance.stat.AttackSpeed.GetValue() + 1)));
        //PlayerMain.Instance.canAttack = true;
        //PlayerMain.Instance.isAttacking = false;
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
