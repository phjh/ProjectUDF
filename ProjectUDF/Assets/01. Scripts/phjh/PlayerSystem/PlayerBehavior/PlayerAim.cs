using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : Player
{
    [SerializeField] Player _player;

    protected void Start()
    {
        _playerStat = _player._playerStat;
    }

    private void SetRotation()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(_inputReader.AimPosition);
        Vector2 dir = (worldMousePos - transform.position);
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void FixedUpdate()
    {
        //if (!_player.IsAttacking)
        {
            SetRotation();
        }
    }

}
