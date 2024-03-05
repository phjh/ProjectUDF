using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private PlayerStat _playerStat;

    private float _currentSpeed;

    private CharacterController _characterController;
    public bool IsGround
    {
        get => _characterController.isGrounded;
    }

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private Quaternion _degreeY45;

    private bool _activeMove = true;
    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    private void Awake()
    {
        _degreeY45 = Quaternion.AngleAxis(45f, Vector3.up);
        _characterController = GetComponent<CharacterController>();
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
        Vector2 move = _degreeY45 * new Vector2(_inputDirection.x, _inputDirection.y);
        _movementVelocity = move * (_currentSpeed * Time.deltaTime);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _playerStat.EditStat(Stats.MoveSpeed, 0.5f);
        }
        
    }
}