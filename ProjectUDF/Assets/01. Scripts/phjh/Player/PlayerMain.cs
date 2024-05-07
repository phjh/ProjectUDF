using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.Serialization;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMain : MonoSingleton<PlayerMain>
{
    public PlayerStats stat;
    public InputReader inputReader;

    public float brightness;
    public Gradient randomColors;
    public Material hitMat;

    public Action OnAttackEvent;

    #region ������ ���� ������

    public PlayerWeapon nowWeapon;

    public GameObject nowWeaponObj { get; private set; }
    public PlayerBaseAttack baseAttack {  get; private set; }
    public PlayerChargeAttack chargeAttack { get; private set; }

    public PlayerSkillAttack nowSkill;

    public PlayerMovement playerMove { get; set; }

    public bool canMove { get; set; }

    public bool canDodging {  get; set; }

    public bool isDodging { get; set; }

    bool isInvincible;

    [Tooltip("�����ð�")]
    public float invincibleTime = 0.4f;

    #endregion

    #region ���� ���� ������


    public PlayerAim playerAim { get; private set; }

    public bool canAttack { get; set; }
    public bool isAttacking { get; set; }
    public bool baseAttackPrepare { get; set; }
    public bool chargeAttackPrepare { get; set; }
    public float recentDamage { get; set; }
    public bool isCritical { get; set; }

    public int nowStone;

    #endregion


    private void Awake()
    {
        GameManager.Instance.player = this;

        if(stat == null)
            Debug.LogError("Stat is null!");

        if(inputReader == null)
            Debug.LogError("input reader is null!");

        if (transform.parent.TryGetComponent<PlayerMovement>(out PlayerMovement move))
            playerMove = move;
        else
            Debug.LogError("Error while get PlayerMovement in Playermain");

        nowStone = 1;

        canMove = true;
        canDodging = true;

        canAttack = true;
        isAttacking = false;
        recentDamage = 4f;
        isCritical = false;

        playerAim = FindObjectOfType<PlayerAim>();
        if (playerAim == null)
            Debug.LogError("Player Aim is not in the Hierarchy");

        stat.SetOwner(this);
        stat = stat.Clone();

        SetWeapon(nowWeapon);
    }

    public int testStone;

    private void Update()
    {
        if (Input.GetMouseButton(0))
            baseAttack.OnAttackPrepare();
        else if (Input.GetMouseButton(1))
            chargeAttack.OnAttackPrepare();

        if (Input.GetKeyDown(KeyCode.G))
        {
            UnEquipStone();
            EquipStone(testStone);
        }
        if (Input.GetKeyDown(KeyCode.H))
            UnEquipStone();
    }

    public void Attack(PlayerAttack attack)
    {
        attack.Attack(attack);
    }

    #region �÷��̾� ü�� ����

    public void GetDamage()
    {
        if (isDodging || isInvincible)
            return;
        //stat.EditPlayerHP(-1);
        Debug.Log(stat.CurHP);
        StartCoroutine(Invincible());
        UIManager.Instance.ShowBloodScreen(invincibleTime);

        if (stat.CurHP <= 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GetHeal()
    {
        stat.EditPlayerHP(1);
    }

    //�¾����� �����Ǵ°�
    private IEnumerator Invincible()
    {
        isInvincible = true;

        float time = 0;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Material baseMat = renderer.material;
        renderer.material = hitMat;

        GameManager.Instance.ShakeCamera();
        while (time < invincibleTime)
        {
            MaterialPropertyBlock mpb = new();
            Color newColor = randomColors.Evaluate(Mathf.Lerp(0,1,(Mathf.Sin(time/invincibleTime * 7)+1)/2)) * brightness;
            mpb.SetColor("_Black", newColor);
            renderer.SetPropertyBlock(mpb);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);                
        }

        renderer.material = baseMat;
        yield return new WaitForSeconds(0.1f);

        isInvincible = false;

    }

    #endregion

    public void SetWeapon(PlayerWeapon weapon)
    {
        nowWeapon = weapon;
        nowWeaponObj = Instantiate(nowWeapon.weaponObj, playerAim.transform);
        if(nowWeaponObj.TryGetComponent<PlayerBaseAttack>(out var baseAtk))
            baseAttack = baseAtk;
        if (nowWeaponObj.TryGetComponent<PlayerChargeAttack>(out var chargeAtk))
            chargeAttack = chargeAtk;

    }

    public void SkillChange(PlayerSkillAttack skill)
    {
        nowSkill = skill;
    }

    #region �÷��̾� ���� - �� ����,����

    public void EquipStone(int stoneType)    //�� �����Ҷ� �θ��� �޼���
    {
        if (nowStone != 0)
            UnEquipStone();

        nowStone = stoneType;
    }

    public void UnEquipStone()  //�� ���� �����Ҷ� �θ��� �޼���
    {
        if (baseAttack.isActiveonce)
            baseAttack.AdditionalAttack[nowStone].Invoke();

        if (chargeAttack.isActiveonce)
            chargeAttack.AdditionalAttack[nowStone].Invoke();

        nowStone = 0;
    }

    #endregion

}
