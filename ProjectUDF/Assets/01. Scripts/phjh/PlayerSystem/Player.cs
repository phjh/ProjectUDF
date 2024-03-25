using System;
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

    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    private void Awake()
    {
        _playerStat = _playerStat.Clone();
        _playerStat.SetOwner(this);
        GameManager.Instance.player = this;
    }

	private void Update()
	{
        
	}
}