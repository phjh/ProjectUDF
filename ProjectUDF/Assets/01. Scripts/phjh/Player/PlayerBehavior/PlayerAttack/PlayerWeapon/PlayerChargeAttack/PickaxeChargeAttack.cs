using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeChargeAttack : PlayerChargeAttack, IStopAttractable
{
    [SerializeField]
    GameObject attackRange;
    [SerializeField]
    Collider2D atkcollider;

    float charged = 0;

    protected override void TryAttack()
    {
        if (!CanAttack())
            return;

        base.TryAttack();
        Debug.Log("attack Invoked");
        OnAttackStart();
    }


    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        _showRange = true;
        attackRange.SetActive(true);
        charged += Time.deltaTime;
        float scale = Mathf.Lerp(1.4f, 1.8f, Mathf.Clamp(charged / maxChargedFactor, 0, 1));
        attackRange.transform.localScale = new Vector3(scale, scale, scale);
    }

    protected override void OnAttackStart()
    {
        float baseFactor = _damageFactor;
        _damageFactor = Mathf.Lerp(_damageFactor, maxChargedFactor, Mathf.Clamp(charged / maxChargedFactor, 0, 1));

        StopAiming();

        //������
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //������ ���ϱ�
        float damage = CalculateDamage();
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
        attackRange.transform.localScale = Vector3.one;
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.isAttacking = false;
        PlayerMain.Instance.canAttack = true;
        charged = 0f;
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
