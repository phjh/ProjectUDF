using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions, IGameSystemActions
{
    public event Action<Vector2> MovementEvent;
    public event Action DodgeEvent;
    public event Action PauseEvent;

    public Vector2 AimPosition { get; private set; } //���콺�� �̺�Ʈ����� �ƴϱ� ������
    private Controls _inputAction;


    private void OnEnable()
    {
        if (_inputAction == null)
        {
            _inputAction = new Controls();
            _inputAction.Player.SetCallbacks(this); //�÷��̾� ��ǲ�� �߻��ϸ� �� �ν��Ͻ��� �������ְ�
            _inputAction.GameSystem.SetCallbacks(this); 
        }
        
        _inputAction.Player.Enable(); //Ȱ��ȭ
        _inputAction.GameSystem.Enable(); //����� ����
    }

    #region Player Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        MovementEvent?.Invoke(value);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(!PlayerMain.Instance.preparingAttack)
            DodgeEvent?.Invoke();
    }

    public void OnBaseAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            PlayerMain.Instance.Attack(PlayerMain.Instance.baseAttack);
        }
    }

    public void OnChargeAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            PlayerMain.Instance.Attack(PlayerMain.Instance.chargeAttack);
        }
    }

    public void OnSKill(InputAction.CallbackContext context)
    {
        if (context.started)
            PlayerMain.Instance.Attack(PlayerMain.Instance.nowSkill);
    }
    #endregion

    #region GameSystem Inputs
    public void OnPause(InputAction.CallbackContext context)
    {
            PauseEvent?.Invoke();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        Debug.LogWarning("tab entered");
        if (context.started)
        {
            Debug.LogWarning("tab entered2");
            UIManager.Instance.ManagePocketUI();
        }
    }


    #endregion
}