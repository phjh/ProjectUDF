using UnityEngine;

public class Player : MonoBehaviour
{
    public InputReader _inputReader;

    public PlayerStat _playerStat;

    [HideInInspector] public CharacterController _characterController;

    protected bool _activeMove = true;
    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    public bool IsGround
    {
        get => _characterController.isGrounded;
    }

    private void Awake()
    {
        _playerStat = _playerStat.Clone(); 
        _characterController = GetComponent<CharacterController>();
    }
}