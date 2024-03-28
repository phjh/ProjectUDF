using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    Coroutine animationCoroutine;

    #region SpineAnimations

    [SpineAnimation]
    public List<string> IdleAnimations;

    [SpineAnimation]
    public List<string> MoveAnimations;

    [SpineAnimation]
    public List<string> leftAttackAnimations;

    [SpineAnimation]
    public List<string> chargingAttack;
    
    [SpineAnimation]
    public List<string> rightAttackAnimations;

    [SpineAnimation]
    public List<string> PickaxeIdleAnimations;

    [SpineAnimation]
    public List<string> DodgeAnimation;

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
        PlayerStat.OnDeadPlayer += OnDie;
        animationCoroutine = StartCoroutine(SetAnimation());
    }


    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    public void OnDie()
    {
        StopCoroutine(animationCoroutine);
        skeletonAnimation.timeScale = 0.5f;
        skeletonAnimation.AnimationState.SetAnimation(0, dieAnimation, false);
        skeletonAnimation.AnimationState.SetAnimation(1, dieAnimation, false);
    }

    IEnumerator SetAnimation()
    {
        float time = 0;
        float fixedTime = 0.01f;
        while (true)
        {
            if(time > 0.8f)
            {
                time = 0;
            }
            else
            {
                time += fixedTime;
            }

            skeletonAnimation.AnimationState.TimeScale = _player._playerStat.MoveSpeed.GetValue() / 3f;

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
                if (_inputDirection == Vector2.zero)
                    skeletonAnimation.AnimationState.SetAnimation(1, IdleAnimations[aimAngle], false).AnimationStart = time;
                else
                {
                    int attackingdir = Mathf.Abs((int)lastMoveDirection - aimAngle);
                    if (attackingdir >=3 && attackingdir <= 5)
                        skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = 0.8f-time;
                    else
                        skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = time;
                }
                if (Input.GetMouseButton(1))
                {
                    aimAngle = aim.Angle;
                    skeletonAnimation.AnimationState.SetAnimation(0, chargingAttack[aimAngle], false).AnimationStart = time;
                }
                else if(Input.GetMouseButtonUp(1))
                {
                    Debug.Log("???");
                    skeletonAnimation.AnimationState.TimeScale = 1.2f;
                    skeletonAnimation.AnimationState.SetAnimation(0, rightAttackAnimations[aimAngle], false).AnimationStart = 0.25f;
                    skeletonAnimation.AnimationState.SetAnimation(1, rightAttackAnimations[aimAngle], false).AnimationStart = 0.25f;
                    yield return new WaitForSeconds(fixedTime * 20);
                }
                yield return new WaitForSeconds(fixedTime);
                continue;
            }

            if (_inputDirection == Vector2.zero)
            {
                skeletonAnimation.AnimationState.SetAnimation(1, IdleAnimations[(int)lastMoveDirection] , false).AnimationStart = time;
                if (!(_player.CanAttack && _player.IsAttacking))
                    skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimations[(int)lastMoveDirection], false).AnimationStart = time;
                yield return new WaitForSeconds(fixedTime);
                continue;
            }

            skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[(int)lastMoveDirection], false).AnimationStart = time;
            if (!(_player.CanAttack && _player.IsAttacking))
                skeletonAnimation.AnimationState.SetAnimation(0, MoveAnimations[(int)lastMoveDirection], false).AnimationStart = time;

            yield return new WaitForSeconds(fixedTime);
        }
    }


}
