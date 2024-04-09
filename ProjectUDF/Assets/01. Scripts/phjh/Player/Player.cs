using System;
using System.Collections;
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

    [Tooltip("무적시간")]
    public float invincibleTime = 0.4f;

    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    private CircleCollider2D _headtrigger;
    private CapsuleCollider2D _bodytrigger;

    private void Awake()
    {
        _playerStat = _playerStat.Clone();
        _playerStat.SetOwner(this);
        _headtrigger = GetComponentInParent<CircleCollider2D>();
        _bodytrigger = GetComponentInParent<CapsuleCollider2D>();
    }

    public void GetDamage()
    {
        if (_isdodgeing)
            return;
        _playerStat.EditPlayerHP(-1);
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
        _headtrigger.enabled = false;
        _bodytrigger.enabled = false;
        yield return new WaitForSeconds(invincibleTime);
        _headtrigger.enabled = true;
        _bodytrigger.enabled = true;
    
    }

}