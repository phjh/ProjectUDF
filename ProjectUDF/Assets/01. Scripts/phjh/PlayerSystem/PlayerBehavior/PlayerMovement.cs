using DG.Tweening;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : Player
{
    [SerializeField] Player _player;
    private Rigidbody2D _rigidbody; 

	private float _currentSpeed;

    private CapsuleCollider2D _colider;
    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;

    public Vector2 lastinputDir = Vector2.down;

    protected void Start()
    {
        _playerStat = _player._playerStat;

        _inputReader.MovementEvent += SetMovement;
        _playerStat.MoveSpeedChanged += LoadMoveSpeed;
        stopImmediately += StopImmediately;
        _inputReader.DodgeEvent += Dodge;
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentSpeed = _playerStat.MoveSpeed.GetValue();        
        _colider = GetComponent<CapsuleCollider2D>();
    }

    private void OnDestroy()
    {
        _inputReader.MovementEvent -= SetMovement;
        _playerStat.MoveSpeedChanged -= LoadMoveSpeed;
        stopImmediately -= StopImmediately;
        _inputReader.DodgeEvent -= Dodge;
    }

    public void Dodge()
    {
        if (!_canDodge || !_player.ActiveMove) return;
        _canDodge = false;
        Debug.Log("a");
        StartCoroutine(DoDodge());
    }

    IEnumerator DoDodge()
    {
        _player._isdodgeing = true;
        _player.ActiveMove = false;
        _rigidbody.velocity = lastinputDir * _currentSpeed * 1.8f;
        _colider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        _colider.enabled = true;
        _player.ActiveMove = true;
        _player._isdodgeing = false;
        yield return new WaitForSeconds(Mathf.Clamp(3f - _playerStat.GetStatByType(Stats.MoveSpeed).GetValue() / 10f, 1, 3));
        _canDodge = true;
    }

    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    public void LoadMoveSpeed(float value)
    {
        _currentSpeed = value;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = _inputDirection * _currentSpeed;
        if (_inputDirection != Vector2.zero)
            lastinputDir = _inputDirection;
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    private void Move()
    {
        _rigidbody.velocity = _movementVelocity;
        //_characterController.Move(_movementVelocity);
    }

    private void FixedUpdate()
    {
        if (_player.ActiveMove)
            CalculatePlayerMovement();
        if (!_player._isdodgeing)
            Move();
    }

    //테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _playerStat.EditModifierStat(Stats.MoveSpeed, 0.5f);
        }

    }

}