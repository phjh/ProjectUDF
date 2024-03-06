using UnityEngine;

public class Player : MonoBehaviour
{
    public InputReader _inputReader;

    public PlayerStat _playerStat;

    [HideInInspector] public CharacterController _characterController;

    public bool IsGround
    {
        get => _characterController.isGrounded;
    }

    private void Awake()
    {
        //_playerStat = _playerStat.Clone();  //=>  클론시 왜 인지 NullRef가 뜨기시작
        _characterController = GetComponent<CharacterController>();
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