using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : Player
{
    [SerializeField] Player _player;

    private float _currentSpeed;

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;

    private bool _activeMove = true;
    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    private void Start()
    {
        _playerStat = _player._playerStat;
        _inputReader = _player._inputReader;
        _characterController = _player._characterController;

        _inputReader.MovementEvent += SetMovement;
        _playerStat.MoveSpeedChanged += LoadMoveSpeed;
        _currentSpeed = _playerStat.PlayerMoveSpeed;        
    }

    private void OnDestroy()
    {
        _inputReader.MovementEvent -= SetMovement;
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
        _movementVelocity = _inputDirection * (_currentSpeed * Time.deltaTime);
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    private void Move()
    {
        _characterController.Move(_movementVelocity);
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
        if (_activeMove)
        {
            CalculatePlayerMovement();
        }
        Move();
        SetRotation();
    }

}