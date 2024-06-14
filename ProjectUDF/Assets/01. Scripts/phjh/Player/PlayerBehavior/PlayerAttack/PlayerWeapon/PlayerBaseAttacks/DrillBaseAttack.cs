using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrillBaseAttack : PlayerBaseAttack, IStopAttractable
{ 
    private GameObject baseAttackRange;
    private Collider2D baseCollider;
    private float moveMultiply = 1;

    PlayerAim aim;
    private bool exceptionAttack = false;

    int lastAimDir;

    float lastTime = 0;

    protected override void Start()
    {
        baseAttackRange = attackRange;
        atkSpeedcol = AttackSpeedStoneObj.GetComponentInChildren<Collider2D>();
        aim = PlayerMain.Instance.playerAim;
        lastTime = Time.time;
        base.Start();
    }

    bool animationSet = false;

    public override void OnAttackPrepare()
    {
        //����ٰ� �帱 �� �߰� �� �������̽��� ���� �޾ƿͼ� �״ٲ����ϱ�
        if(Time.time - lastTime < Time.fixedDeltaTime && PlayerMain.Instance.preparingAttack)
        {
            Debug.LogWarning("drill return invoked");
            StopAiming();
            return;
        }
        else
        {
            StartAiming();
        }

        if (PlayerMain.Instance.playerMove == null)
            Debug.LogError("move is null");

        if (PlayerMain.Instance.isDodging)
        {
            Debug.Log("Dodging");
            return;
        }

        InvokeStoneAttack();

        PlayerMain.Instance.preparingAttack = true;

        //���ݹ��� ǥ��
        attackRange.gameObject.SetActive(true);

        if (!animationSet || (lastAimDir != aim.Angle || aim.Angle == 0))
        {
            SpineAnimator.Instance.SetSortedAnimation(skele_Animator, AttackingAnimation, aim.Angle, 0, true, Time.time % AttackingAnimation[0].Animation.Duration);
            SpineAnimator.Instance.SetSortedAnimation(skele_Animator, AttackingAnimation, aim.Angle, 1, true, Time.time % AttackingAnimation[0].Animation.Duration);
            SpineAnimator.Instance.isEmpty = true;
            animationSet = true;
            lastTime = Time.time;
        }

        //������
        PlayerMain.Instance.canAttack = false;

        //������ ���ϱ�
        float damage = CalculateDamage(damageFactor);
        Debug.Log("damage : " + damage);

        if (exceptionAttack)
        {

            return;
        }

        //����Ʈ ���
        //EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 3, 0.4f);
        PlayerMain.Instance.playerMove.SetFixedDir(true, PlayerMain.Instance.playerAim.Mousedir.normalized * moveMultiply);

        //������ ����
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
        SpineAnimator.Instance.SetEmptyAnimation(skele_Animator, 1, 0.1f);
        attackRange.gameObject.SetActive(false);
        animationSet = false;
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
    [Tooltip("���ǵ� ���� ���� ���� ���")]
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
        //�÷��̾� ��¡����ó�� �ٲ��ش�
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
        //����
        Debug.Log("�帱 �̼ӱ��� ȿ���� �����");
    }

    public void StartAiming()
    {
        PlayerMain.Instance.playerAim.enabled = true;
    }

    public void StopAiming()
    {
        PlayerMain.Instance.playerAim.enabled = false;
        Debug.LogWarning("aimtracking stoped");
    }
}
