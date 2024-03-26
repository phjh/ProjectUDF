using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirectionList
{
    Front = 0,
    Rightfront = 1,
    Right = 2,
    RightBack = 3,
    Back = 4,
    LeftBack = 5,
    Left = 6,
    Leftfront = 7,
}

public class PlayerAnimation : Player
{
    [SerializeField] Player _player;

    SkeletonAnimation skeletonAnimation;

    MoveDirectionList lastMoveDirection;

    [SerializeField]
    PlayerAim aim;

    #region SpineAnimations

    [SpineAnimation]
    public List<string> IdleAnimations;

    [SpineAnimation]
    public List<string> MoveAnimations;

    [SpineAnimation]
    public string leftAttackAnimation;
    
    [SpineAnimation]
    public List<string> rightAttackAnimation;

    [SpineAnimation]
    public string DodgeAnimation;

    [SpineAnimation]
    public string hitAnimation;

    [SpineAnimation]
    public string dieAnimation;

    #endregion

    

    public Vector2 _inputDirection;

    protected void Start()
    {
        _playerStat = _player._playerStat;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        _inputReader.MovementEvent += SetMovement;
    }

    private void OnDestroy()
    {
        _inputReader.MovementEvent -= SetMovement;
    }

    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    public void SetAnimation()
    {
        bool isRight = _inputDirection.x > 0;
        bool isUp = _inputDirection.y > 0;

        if(_inputDirection == Vector2.zero)
        {
            //다른곳 바라보지 않게 막는용
        }
        else if(Mathf.Abs(_inputDirection.x) > Mathf.Abs(_inputDirection.y))
        {
            lastMoveDirection = isRight ? MoveDirectionList.Right : MoveDirectionList.Left;
            //skeletonAnimation.AnimationName = isRight ? moverightAnimation : moveleftAnimation;
        }
        else if (Mathf.Abs(_inputDirection.x) < Mathf.Abs(_inputDirection.y))
        {
            lastMoveDirection = isUp ? MoveDirectionList.Back : MoveDirectionList.Front;
            //skeletonAnimation.AnimationName = isUp ? moveupAnimation : movedownAnimation;
        }
        else if (isRight)
        {
            lastMoveDirection = isUp ? MoveDirectionList.RightBack : MoveDirectionList.Rightfront;
            //skeletonAnimation.AnimationName = isUp ? moverightupAnimation : moverightdownAnimation;
        }
        else if(!isRight)
        {
            lastMoveDirection = isUp ? MoveDirectionList.LeftBack : MoveDirectionList.Leftfront;
            //skeletonAnimation.AnimationName = isUp ? moveleftupAnimation : moveleftdownAnimation;
        }

        if (_player.CanAttack && _player.IsAttacking)
        {
            skeletonAnimation.AnimationName = rightAttackAnimation[aim.Angle];
            if (Input.GetMouseButton(1))
            {
                skeletonAnimation.AnimationName = rightAttackAnimation[(int)lastMoveDirection];
                skeletonAnimation.timeScale = 0f;
            }
            else
            {
                skeletonAnimation.timeScale = 1.2f;
                Debug.Log("???");
            }
            return;
        }

        if (_inputDirection == Vector2.zero)
        {
            skeletonAnimation.AnimationName = IdleAnimations[(int)lastMoveDirection];
            return;
        }

        skeletonAnimation.AnimationName = MoveAnimations[(int)lastMoveDirection];
    }

    public void SetAttackDirection()
    {

    }

    private void FixedUpdate()
    {
        SetAnimation();
    }

}
