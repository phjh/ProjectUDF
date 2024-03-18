using UnityEngine;

public class PlayerMovement : Player
{
    [SerializeField] Player _player;

    private float _currentSpeed;

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;

    Rigidbody2D _rigidbody;

    protected void Start()
    {
        _playerStat = _player._playerStat;

        _inputReader.MovementEvent += SetMovement;
        _playerStat.MoveSpeedChanged += LoadMoveSpeed;
        stopImmediately += StopImmediately;
        _currentSpeed = _playerStat.MoveSpeed.GetValue();
        _inputReader.DodgeEvent += Dodge;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Dodge()
    {

    }

    private void OnDestroy()
    {
        _inputReader.MovementEvent -= SetMovement;
        _playerStat.MoveSpeedChanged -= LoadMoveSpeed;
        stopImmediately -= StopImmediately;
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
        _rigidbody.velocity = _movementVelocity;
        //_characterController.Move(_movementVelocity);
    }

    private void FixedUpdate()
    {
        if (_player.ActiveMove)
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
            _playerStat.IncreaseStatBy(1, 0, Stats.MoveSpeed);
        }

    }

}