using System;
using System.Collections;
using Unity.VisualScripting;
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
    private Controls _playerInputAction;
    private Controls _gameSystemAction;

    private void OnEnable()
    {
        if (_playerInputAction == null)
        {
            _playerInputAction = new Controls();
            _playerInputAction.Player.SetCallbacks(this); //�÷��̾� ��ǲ�� �߻��ϸ� �� �ν��Ͻ��� �������ְ�
        }
        
        if(_gameSystemAction == null)
        {
            _gameSystemAction = new Controls();
            _gameSystemAction.Player.SetCallbacks(this);
        }

        _playerInputAction.Player.Enable(); //Ȱ��ȭ
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
    #endregion

    #region GameSystem Inputs
    public void OnPause(InputAction.CallbackContext context)
    {
        PauseEvent?.Invoke();
    }
    #endregion
}