using UnityEngine;

public class PlayerMovement : Player
{
    [SerializeField] Player _player;

    private float _currentSpeed;

    Rigidbody2D _rigidbody;

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;

    protected void Start()
    {
        _playerStat = _player._playerStat;

        _inputReader.MovementEvent += SetMovement;
        _playerStat.MoveSpeedChanged += LoadMoveSpeed;
        stopImmediately += StopImmediately;
        _inputReader.DodgeEvent += Dodge;
        _currentSpeed = _playerStat.PlayerMoveSpeed;        
        _rigidbody = GetComponent<Rigidbody2D>();   
    }

    private void OnDestroy()
    {
        _inputReader.MovementEvent -= SetMovement;
        _playerStat.MoveSpeedChanged -= LoadMoveSpeed;
        stopImmediately -= StopImmediately;
        _inputReader.DodgeEvent -= Dodge;
    }

    void Dodge()
    {


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
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    private void Move()
    {
        //_characterController.Move(_movementVelocity);
        _rigidbody.velocity = MovementVelocity;
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
            _playerStat.EditStat(Stats.MoveSpeed, 0.5f);
        }

    }

}