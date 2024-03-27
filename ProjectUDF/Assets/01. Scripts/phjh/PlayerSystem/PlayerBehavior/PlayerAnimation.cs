using Spine;
using Spine.Unity;
using System.Collections;
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

    public List<AnimationReferenceAsset> chargingAttack;

    #region SpineAnimations

    [SpineAnimation]
    public List<string> IdleAnimations;

    [SpineAnimation]
    public List<string> MoveAnimations;

    [SpineAnimation]
    public List<string> leftAttackAnimations;
    
    [SpineAnimation]
    public List<string> rightAttackAnimations;

    [SpineAnimation]
    public List<string> PickaxeIdleAnimations;

    [SpineAnimation]
    public string DodgeAnimation;

    [SpineAnimation]
    public string hitAnimation;

    [SpineAnimation]
    public string dieAnimation;

    #endregion

    int aimAngle = 0;

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

    void SetAnimation()
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
            if (Input.GetMouseButton(1))
            {
                aimAngle = aim.Angle;
                skeletonAnimation.AnimationState.SetAnimation(1, chargingAttack[aimAngle], true);
            }
            else
            {
                skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
                skeletonAnimation.AnimationState.SetAnimation(0, rightAttackAnimations[aimAngle], false);
                skeletonAnimation.AnimationState.AddAnimation(0, PickaxeIdleAnimations[aimAngle], true, 0);
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

    private void FixedUpdate()
    {
        SetAnimation();
    }

}
