using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBehaviour : MonoBehaviour
{
    private Vector2 _moveDir;

    private void OnEnable()
    {
        PlayerMain.Instance.inputReader.MovementEvent += SetMovement;

    }

    private void OnDisable()
    {
        PlayerMain.Instance.inputReader.MovementEvent -= SetMovement;
        
    }

    private void Update()
    {
        if (PlayerMain.Instance.canMove == false)
            return;



    }

    private void SetMovement(Vector2 value)//InputSystem을 통해 여기서 움직임을 받는다
    {
        _moveDir = value;
    }
    
    public void StopImmediately()//즉시정지
    {
        _moveDir = Vector2.zero;
    }

}
