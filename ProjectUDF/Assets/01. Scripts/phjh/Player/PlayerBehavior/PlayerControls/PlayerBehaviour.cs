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

    private void SetMovement(Vector2 value)//InputSystem�� ���� ���⼭ �������� �޴´�
    {
        _moveDir = value;
    }
    
    public void StopImmediately()//�������
    {
        _moveDir = Vector2.zero;
    }

}
