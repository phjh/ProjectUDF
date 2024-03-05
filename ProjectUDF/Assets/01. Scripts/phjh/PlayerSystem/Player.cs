using UnityEngine;

[RequireComponent(typeof(CharacterController))]
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
        _playerStat = _playerStat.Clone();
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