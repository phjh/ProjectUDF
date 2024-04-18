using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider slider;

    protected void OnEnable()
    {
        _playerStat = _player._playerStat;
        GameManager.Instance.UpdateState(GameStates.Start);
        _inputReader.MovementEvent += SetMovement;
        stopImmediately += StopImmediately;
        _inputReader.DodgeEvent += Dodge;
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentSpeed = _playerStat.MoveSpeed.GetValue();        
        _colider = GetComponent<CapsuleCollider2D>();
    }

    private void OnDisable()
    {
        _inputReader.MovementEvent -= SetMovement;
        stopImmediately -= StopImmediately;
        _inputReader.DodgeEvent -= Dodge;
    }

    public void Dodge()
    {
        if (!_canDodge || !_player.ActiveMove) return;
        _canDodge = false;
        StartCoroutine(DoDodge());
    }

    IEnumerator DoDodge()
    {
        _player._isdodgeing = true;
        _player.ActiveMove = false;
        _rigidbody.velocity = lastinputDir * _currentSpeed * 1.8f;
        slider.value = 0;
        slider.gameObject.SetActive(true);
        StartCoroutine(DodgeCooltimeSet());
        yield return new WaitForSeconds(0.5f);
        _player.ActiveMove = true;
        _player._isdodgeing = false;
    }

    IEnumerator DodgeCooltimeSet()
    {
        float cooltime = DodgeCooltime() + 0.5f;
        float time = 0;
        while(time <= cooltime)
        {
            time+= Time.deltaTime;
            slider.value = Mathf.Lerp(0,1,time/cooltime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if(slider.value != 1) 
            slider.value = 1;
        slider.gameObject.SetActive(false);
        _canDodge = true;
    }

    public float DodgeCooltime() => Mathf.Clamp(3 - _player._playerStat.MoveSpeed.GetValue() / 10, 1, 3);

    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalculatePlayerMovement()
    {
        _currentSpeed = 4 + _player._playerStat.MoveSpeed.GetValue()/5;
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
    }

    private void FixedUpdate()
    {
        if (_player.ActiveMove)
            CalculatePlayerMovement();
        else 
            StopImmediately();
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            GetHeal();
            Debug.Log(1);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            _player.GetDamage();
            Debug.Log("Get Damaged");
        }
    }
}