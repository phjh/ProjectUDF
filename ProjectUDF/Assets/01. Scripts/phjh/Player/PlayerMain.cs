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

    #region 움직임 관련 변수들

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

    [Tooltip("무적시간")]
    public float invincibleTime = 0.4f;

    #endregion

    #region 공격 관련 변수들


    public PlayerAim playerAim { get; private set; }

    public bool canAttack { get; set; }
    public bool isAttacking { get; set; }
    public bool baseAttackPrepare { get; set; }
    public bool chargeAttackPrepare { get; set; }
    public float recentDamage { get; set; }
    public bool isCritical { get; set; }

    public int EquipMainOre;

	#endregion

	private void OnEnable()
	{
		OreInventory.Instance.OnChangeMainOre += EquipStone;
	}

	private void OnDisable()
	{
		OreInventory.Instance.OnChangeMainOre -= EquipStone;
	}

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

        EquipMainOre = 1;

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

    private void Update()
    {
        if (Input.GetMouseButton(0))
            baseAttack.OnAttackPrepare();
        else if (Input.GetMouseButton(1))
            chargeAttack.OnAttackPrepare();
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            UnEquipStone();
            EquipStone();
        }
        if (Input.GetKeyDown(KeyCode.H))
            UnEquipStone();*/
    }

	#region Methods

	public void Attack(PlayerAttack attack)
    {
        attack.Attack(attack);
    }

    #region 플레이어 체력 관련

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

    //맞았을때 무적되는거
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

	#region 플레이어 공격 - 돌 장착,해제

	private void EquipStone(Stats statName)
	{
		EquipMainOre = (int)statName;
        if(statName == Stats.None) UnEquipStone();
	}


	public void UnEquipStone()  //돌 장착 해제할때 부르는 메서드
    {
        if (baseAttack.isActiveonce)
            baseAttack.AdditionalAttack[EquipMainOre].Invoke();

        if (chargeAttack.isActiveonce)
            chargeAttack.AdditionalAttack[EquipMainOre].Invoke();
    }

    #endregion

	#endregion
}
