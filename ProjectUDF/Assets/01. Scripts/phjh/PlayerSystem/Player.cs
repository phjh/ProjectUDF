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
        //_playerStat = _playerStat.Clone();  //=>  Ŭ�н� �� ���� NullRef�� �߱����
        _characterController = GetComponent<CharacterController>();
    }

    //�׽�Ʈ��
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _playerStat.EditStat(Stats.MoveSpeed, 0.5f);
        }

    }
}