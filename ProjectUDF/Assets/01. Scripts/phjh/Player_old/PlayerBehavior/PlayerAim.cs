using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public int Angle;

    public float RotZ = 0;

    protected void Start()
    {
        //_playerStat = _player._playerStat;
    }

    private void SetRotation()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(PlayerMain.Instance.inputReader.AimPosition);
        Vector2 dir = (worldMousePos - transform.position);
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        RotZ = rotZ;

        int angle = (int)((rotZ + 90) / 22.5f);
        if (angle < 0)
        {
            angle = 15 + angle;
        }
        Angle = (angle + 1) / 2;
    }

    private void FixedUpdate()
    {
        //if (!_player.IsAttacking)
        {
            SetRotation();
        }
    }

}
