using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : Player
{
    [SerializeField] Player _player;

    SkeletonAnimation skeletonAnimation;

        #region SpineAnimations

        [SpineAnimation]
    public string moverightAnimation;

    [SpineAnimation]
    public string moveleftAnimation;

    [SpineAnimation]
    public string moveupAnimation;

    [SpineAnimation]
    public string movedownAnimation;

    [SpineAnimation]
    public string moverightupAnimation;

    [SpineAnimation]
    public string moveleftupAnimation;

    [SpineAnimation]
    public string moverightdownAnimation;

    [SpineAnimation]
    public string moveleftdownAnimation;

    #endregion

    private Vector2 _inputDirection;

    protected void Start()
    {
        _playerStat = _player._playerStat;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        _inputReader.MovementEvent += SetMovement;
    }

    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void FixedUpdate()
    {

    }

}
