using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public enum MoveAnimationList
{
    FrontIdle = 0,
    RightfrontIdle = 1,
    RightIdle = 2,
    RightupIdle = 3,
    UpIdle = 4,
    LeftupIdle = 5,
    LeftIdle = 6,
    LeftfrontIdle =7,
    FrontMove = 8,
    FrontrightMove = 9,
    RightMove = 10,
    RightupMove = 11,
    UpMove = 12,
    LeftupMove = 13,
    LeftMove = 14,
    LeftfrontMove = 15

}

public class PlayerAnimation : Player
{
    [SerializeField] Player _player;

    SkeletonAnimation skeletonAnimation;

    #region SpineAnimations

    [SpineAnimation]
    public List<string> MoveAnimations;



    [SpineAnimation]
    public string Idle;

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

    public Vector2 _inputDirection;

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

    public void SetMoveAnimation()
    {
        if (_inputDirection == Vector2.zero)
        {
            skeletonAnimation.AnimationName = Idle;
            return;
        }

        bool isRight = _inputDirection.x > 0;
        bool isUp = _inputDirection.y > 0;

         if (Mathf.Abs(_inputDirection.x) < Mathf.Abs(_inputDirection.y))
        { 
            skeletonAnimation.AnimationName = isUp ? moveupAnimation : movedownAnimation;
        }
        else if(Mathf.Abs(_inputDirection.x) > Mathf.Abs(_inputDirection.y))
        {
            skeletonAnimation.AnimationName = isRight ? moverightAnimation : moveleftAnimation;
        }
        else if (isRight)
        {
            skeletonAnimation.AnimationName = isUp ? moverightupAnimation : moverightdownAnimation;
        }
        else if(!isRight)
        {
            skeletonAnimation.AnimationName = isUp ? moveleftupAnimation : moveleftdownAnimation;
        }


    }

    private void FixedUpdate()
    {
        SetMoveAnimation();
    }

}
