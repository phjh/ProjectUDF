using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : Player
{
    [SerializeField] Player _player;

    private float _currentSpeed;

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;

    protected void Start()
    {
        _playerStat = _player._playerStat;

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

    private void FixedUpdate()
    {
        if (_activeMove)
        {
            CalculatePlayerMovement();
        }
        Move();
    }

    //테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _playerStat.EditStat(Stats.MoveSpeed, 0.5f);
        }

    }

}