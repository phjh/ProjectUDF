using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody; 

	private float _currentSpeed;

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;

    public Vector2 lastinputDir = Vector2.down;

    public Slider slider;

    private bool isFixed = false;

    protected void OnEnable()
    {
        GameManager.Instance.UpdateState(GameStates.Start);
        PlayerMain.Instance.inputReader.MovementEvent += SetMovement;
        //stopImmediately += StopImmediately;
        PlayerMain.Instance.inputReader.DodgeEvent += Dodge;
        _rigidbody = GetComponent<Rigidbody2D>();
        Debug.Log(_rigidbody);
        _currentSpeed = PlayerMain.Instance.stat.MoveSpeed.GetValue();
    }

    private void OnDisable()
    {
        //PlayerMain.Instance.inputReader.MovementEvent -= SetMovement;
        //PlayerMain.Instance.inputReader.DodgeEvent -= Dodge;
        //stopImmediately -= StopImmediately;
    }

    public void Dodge()
    {
        if (!PlayerMain.Instance.canDodging || !PlayerMain.Instance.canMove) 
            return;

        PlayerMain.Instance.canDodging = false;
        StartCoroutine(DoDodge());
    }

    IEnumerator DoDodge()
    {   
        PlayerMain.Instance.isDodging = true;
        PlayerMain.Instance.canMove = false;
        _rigidbody.velocity = lastinputDir * _currentSpeed * 1.8f;
        slider.value = 0;
        slider.gameObject.SetActive(true);
        StartCoroutine(DodgeCooltimeSet());
        yield return new WaitForSeconds(0.5f);
        PlayerMain.Instance.isDodging = false;
        PlayerMain.Instance.canMove = true;
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
        PlayerMain.Instance.canDodging = true;
    }

    public float DodgeCooltime() => Mathf.Clamp(3 - PlayerMain.Instance.stat.MoveSpeed.GetValue() / 10, 1, 3);

    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalculatePlayerMovement()
    {
        _currentSpeed = 4 + PlayerMain.Instance.stat.MoveSpeed.GetValue()/5;
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
        if (PlayerMain.Instance.canMove && !PlayerMain.Instance.IsUIPopuped)
            CalculatePlayerMovement();
        else 
            StopImmediately();
        if (!(PlayerMain.Instance.isDodging || isFixed))
            Move();
    }

    //테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerMain.Instance.stat.EditModifierStat(Stats.MoveSpeed, 0.5f);
        }
    }

    public void SetFixedDir(bool onoff, Vector2 dir)
    {
        isFixed = onoff;
        lastinputDir = dir;
        _rigidbody.velocity = dir;
    }

}