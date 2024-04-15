using Spine.Unity;
using System;
using System.Collections;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InputReader _inputReader;

    public PlayerStat _playerStat;

    public Action stopImmediately;

    public bool IsAttacking = false;

    public bool CanAttack = true;

    protected bool _activeMove = true;

    public bool _isdodgeing = false;
    public bool _canDodge = true;

    [SerializeField]
    [Tooltip("무적시간")]
    private float invincibleTime = 0.4f;
    [SerializeField]
    [Tooltip("무적 깜빡이는 속도")]
    private float invincibleSpeed = 6f;

    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    private CapsuleCollider2D _bodytrigger;

    private void Awake()
    {
        _playerStat = _playerStat.Clone();
        _playerStat.SetOwner(this);
        _bodytrigger = GetComponentInParent<CapsuleCollider2D>();
    }

    public void GetDamage()
    {
        if (_isdodgeing)
            return;
        //_playerStat.EditPlayerHP(-1);
        Debug.Log(_playerStat.CurHP);
        StartCoroutine(Invincible());

        if (_playerStat.CurHP <= 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GetHeal()
    {
        _playerStat.EditPlayerHP(1);
    }

    IEnumerator Invincible()
    {

        _bodytrigger.enabled = false;

        if (TryGetComponent<SkeletonAnimation>(out SkeletonAnimation skel))
        {
            Spine.Skeleton skeleton = skel.skeleton;

            float time = 0;

            while(time < invincibleTime)
            {
                float alpha = Mathf.Clamp((Mathf.Sin(time * invincibleSpeed) + 1) / 2, 0.2f, 1f);
                skeleton.A = alpha;
                time += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            skeleton.A = 1;
        }

        _bodytrigger.enabled = true;
    
    }

}