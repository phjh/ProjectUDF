using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

public class PlayerMain : MonoSingleton<PlayerMain>
{
    public PlayerStats stat { get; private set; }
    public InputReader inputReader { get; private set; }

    public Action OnAttackEvent;

    #region 움직임 관련 변수들

    public bool canMove { get; set; }

    public bool canDodging {  get; set; }

    public bool isDodging { get; set; }

    bool isInvincible;

    [Tooltip("무적시간")]
    private float invincibleTime = 0.4f;

    [Tooltip("무적 깜빡이는 속도")]
    private float invincibleSpeed = 6f;

    private Rigidbody2D rb;

    #endregion

    #region 공격 관련 변수들


    public PlayerAim playerAim { get; private set; }

    public bool canAttack { get; set; }
    public bool isAttacking { get; set; }
    public float recentDamage { get; set; }
    public bool isCritical { get; set; }

    #endregion

    public void Init(PlayerAim aim, PlayerStats stat, InputReader reader)
    {
        playerAim = aim;
        this.stat = stat;
        inputReader = reader;
    }

    private void Awake()
    {
        canMove = true;
        canDodging = true;

        canAttack = true;
        isAttacking = false;
        recentDamage = 4f;
        isCritical = false;
    }


    public void Attack(PlayerAttack attack)
    {
        attack.Attack(attack);
    }

    public void GetDamage()
    {
        if (isDodging || isInvincible)
            return;
        stat.EditPlayerHP(-1);
        Debug.Log(stat.CurHP);
        StartCoroutine(Invincible());

        if (stat.CurHP <= 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GetHeal()
    {
        stat.EditPlayerHP(1);
    }

    private IEnumerator Invincible()
    {
        isInvincible = true;

        if (TryGetComponent<SkeletonAnimation>(out SkeletonAnimation skel))
        {
            Spine.Skeleton skeleton = skel.skeleton;

            float time = 0;

            while (time < invincibleTime)
            {
                float alpha = Mathf.Clamp((Mathf.Sin(time * invincibleSpeed) + 1) / 2, 0.2f, 1f);
                skeleton.A = alpha;
                time += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            skeleton.A = 1;
        }
        else
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        isInvincible = false;
    }

}
