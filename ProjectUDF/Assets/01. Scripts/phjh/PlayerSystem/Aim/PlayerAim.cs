using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : Player
{
    [SerializeField] Player _player;

    private float _attackSpeed;

    private void Awake()
    {
        _playerStat = _player._playerStat;
        _inputReader = _player._inputReader;
        _characterController = _player._characterController;
    }

    private void Start()
    {
        _attackSpeed = _playerStat.PlayerAttackSpeed;
    }

    private void FixedUpdate()
    {
        
    }

}
