using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickaxeBaseAttack : PlayerBaseAttack, IStopAttractable
{
    public List<Image> objs;
    
    List<Stats> luckStones = new();
    float baseFactor;
    
    protected override void Start()
    {
        base.Start();
        baseFactor = damageFactor;
    }

    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        InvokeStoneAttack();

        stoneActived = true;

        //���� ���� ǥ��
        SpineAnimator.Instance.SetSortedAnimation(skele_Animator, AttackPrepareAnimation, PlayerMain.Instance.playerAim.Angle, 0);
        PlayerMain.Instance.preparingAttack = true;
        Debug.Log("prepare");
        _showRange = true;
        attackRange.gameObject.SetActive(true);
    }

    protected override void OnAttackStart()
    {
        if (!_showRange)
        {
            Debug.LogWarning("Range is not enabled");
            return;
        }

        SpineAnimator.Instance.SetEmptyAnimation(skele_Animator, 1, AttackingAnimation[0].Animation.Duration);
        SpineAnimator.Instance.SetSortedAnimation(skele_Animator, AttackingAnimation, PlayerMain.Instance.playerAim.Angle, 0);

        //������
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //������ ���ϱ�
        float damage = CalculateDamage(damageFactor);
        Debug.Log("damage : " + damage);

        //���ݹ��� ����
        StopAiming();

        //����Ʈ ���
        EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 3, 0.4f);

        //������ ����
        PlayerMain.Instance.canMove = false;
        
        atkcollider.enabled = true;

        base.OnAttackStart();
    }

    protected override void OnAttacking()
    {
        //���� ���ۿ��� ���� ó�� ���õ� �͵� �����ϱ�
        StartAiming();

        Debug.Log("onattacking");
        PlayerMain.Instance.canMove = true;
        atkcollider.enabled = false;
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.preparingAttack = false;

        base.OnAttacking();
    }

    protected override void OnAttackEnd()
    {
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
        stoneActived = false;
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

    [SerializeField]
    [Tooltip("���ǵ� ���� ���� ���� ���")]
    private float StrengthStoneMultiply;
    protected override void StrengthStoneAttack()
    {
        if (!isActiveonce)
        {
            attackRange.transform.localScale = attackRange.transform.localScale * StrengthStoneMultiply;
            isActiveonce = true;
        }
        else
        {
            attackRange.transform.localScale = attackRange.transform.localScale / StrengthStoneMultiply;
            isActiveonce = false;
        }
    }

    protected override void LuckyStoneAttack()
    {
        if (luckStones.Count < 3)
        {
            int randomStone = UnityEngine.Random.Range(0, 4);
            luckStones.Add((Stats)randomStone);
            objs[luckStones.Count-1].sprite = UIManager.Instance.OreDatas[randomStone].OreSprite;
            objs[luckStones.Count-1].gameObject.SetActive(true);
        }
    }


    [SerializeField]
    [Tooltip("���ӵ� ���� ũ���� ���")]
    private float AttackSpeedStoneFactor;
    [SerializeField]
    [Tooltip("���ӵ� �޺� �ʱ�ȭ �ð�")]
    private float attackSpeedStoneComboResetTime;
    int nowattack;
    float beforeTime;
    protected override void AttackSpeedStoneAttack()
    {
        damageFactor = baseFactor;
        if(beforeTime + attackSpeedStoneComboResetTime < Time.time) //�޺�����
        {
            nowattack = 0;
            beforeTime = Time.time;
            nowattack++;
        }
        else
        {
            if(nowattack == 2)
            {
                damageFactor += AttackSpeedStoneFactor;
                nowattack = 0;
            }
            else
            {
                nowattack++;
                beforeTime = Time.time;
            }
        }

    }

    protected override void MoveSpeedStoneAttack()
    {
        Debug.Log("��Ŭ�� �̼� ȿ���� �����!");
    }

    public override List<Stats> GetLuckyStones()
    {
        return luckStones;
    }

    public override void SetLuckyStones(List<Stats> statList)
    {
        //luckStones = statList;
        luckStones.Clear();
        for (int i = 0; i < 3; i++) 
        {
            objs[i].sprite = null;
            objs[i].gameObject.SetActive(false);
            //objs[i].sprite = UIManager.Instance.OreDatas[(int)statList[i]].OreSprite;
        }
    }

}
