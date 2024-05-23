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
    public event Action InventoryEvent;

    public Vector2 AimPosition { get; private set; } //마우스는 이벤트방식이 아니기 때문에
    private Controls _playerInputAction;
    private Controls _gameSystemAction;


    private void OnEnable()
    {
        if (_playerInputAction == null)
        {
            _playerInputAction = new Controls();
            _playerInputAction.Player.SetCallbacks(this); //플레이어 인풋이 발생하면 이 인스턴스를 연결해주고
        }
        
        if(_gameSystemAction == null)
        {
            _gameSystemAction = new Controls();
            _gameSystemAction.Player.SetCallbacks(this);
        }

        _playerInputAction.Player.Enable(); //활성화
        _playerInputAction.GameSystem.Enable(); //까먹지 말자
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
        if (context.performed)
        {
            Debug.Log("TTTT");
            UIManager.Instance.ManagePocketUI();
            InventoryEvent.Invoke();
        }
    }


    #endregion
}