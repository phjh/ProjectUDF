using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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

public class PlayerAnimation : MonoBehaviour
{
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

    private List<AnimationReferenceAsset> leftAttackAnimations;

    private List<AnimationReferenceAsset> chargingAttack;
    
    private List<AnimationReferenceAsset> rightAttackAnimations;

    private List<AnimationReferenceAsset> WeaponIdleAnimations;

    [SpineAnimation]
    public List<string> DodgeAnimation;

    [SpineAnimation]
    public string dieAnimation;

    #endregion

    public int aimAngle = 0;

    public Vector2 _inputDirection;
    
    float timescale;

    protected void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        PlayerMain.Instance.inputReader.MovementEvent += SetMovement;
        PlayerStats.OnDeadPlayer += OnDie;
        timescale = skeletonAnimation.timeScale;
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




    bool isLeftPressed = false;
    bool isRightPressed = false;
    IEnumerator SetAnimation()
    {
        float time = 0;
        float fixedTime = 0.01f;
        while (true)
        {
            //애니메이션 재생/역재생용
            if(time > 0.8f)
            {
                time = 0;
            }
            else
            {
                time += fixedTime;
            }

            skeletonAnimation.AnimationState.TimeScale = PlayerMain.Instance.stat.MoveSpeed.GetValue() / 3f;

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

            //회피 애니메이션
            if (PlayerMain.Instance.isDodging)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, DodgeAnimation[(int)lastMoveDirection], false);
                skeletonAnimation.AnimationState.SetAnimation(1, DodgeAnimation[(int)lastMoveDirection], false);
                yield return new WaitForSeconds(0.5f);
            }

            if (Input.GetMouseButton(0)&& PlayerMain.Instance.canAttack)
            {
                isLeftPressed = true;
                aimAngle = aim.Angle;
                if (_inputDirection == Vector2.zero)
                {
                    skeletonAnimation.AnimationState.SetAnimation(1, IdleAnimations[aimAngle], false).AnimationStart = time;
                    skeletonAnimation.AnimationState.SetAnimation(0, chargingAttack[aimAngle], false).AnimationStart = time;

                }
                else
                {
                    int attackingdir = Mathf.Abs((int)lastMoveDirection - aimAngle);
                    if (attackingdir >= 3 && attackingdir <= 5)
                    {
                        skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = 0.8f - time;
                        skeletonAnimation.AnimationState.SetAnimation(0, chargingAttack[aimAngle], false).AnimationStart = 0.8f - time;
                    }
                    else
                    {
                        skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = time;
                        skeletonAnimation.AnimationState.SetAnimation(0, chargingAttack[aimAngle], false).AnimationStart = time;
                    }
                }
                yield return new WaitForSeconds(fixedTime);
                continue;
            }
            else if(isLeftPressed)
            {
                skeletonAnimation.AnimationState.TimeScale = 1.2f;
                isLeftPressed = false;
                lastMoveDirection = (MoveDirectionList)aimAngle;
                skeletonAnimation.AnimationState.SetAnimation(0, rightAttackAnimations[aimAngle], false).AnimationStart = 0.25f;
                skeletonAnimation.AnimationState.SetAnimation(1, rightAttackAnimations[aimAngle], false).AnimationStart = 0.25f;
                skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0, 0);
                yield return new WaitForSeconds(0.5f);
            }

            if (Input.GetMouseButton(1) && PlayerMain.Instance.canAttack)
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

                aimAngle = aim.Angle;
                skeletonAnimation.AnimationState.SetAnimation(0, WeaponIdleAnimations[aimAngle], false).AnimationStart = time;
                isRightPressed = true;

                yield return new WaitForSeconds(fixedTime);
                continue;
            }
            else if(isRightPressed)
            {
                skeletonAnimation.AnimationState.TimeScale = 1.2f;
                skeletonAnimation.AnimationState.SetAnimation(0, leftAttackAnimations[aimAngle], false).TimeScale = 0.5f;
                skeletonAnimation.AnimationState.SetAnimation(1, leftAttackAnimations[aimAngle], false).TimeScale = 0.5f;
                skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0, 0);
                yield return new WaitForSeconds(0.7f);
                lastMoveDirection = (MoveDirectionList)aimAngle;
                isRightPressed = false;
            }

            if (_inputDirection == Vector2.zero)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimations[(int)lastMoveDirection] , false).AnimationStart = time;
                skeletonAnimation.AnimationState.SetAnimation(1, IdleAnimations[(int)lastMoveDirection] , false).AnimationStart = time;
                //if (!(_player.CanAttack && _player.IsAttacking))
                //    skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimations[(int)lastMoveDirection], false).AnimationStart = time;
                yield return new WaitForSeconds(fixedTime);
                continue;
            }

            skeletonAnimation.AnimationState.SetAnimation(0, MoveAnimations[(int)lastMoveDirection], false).AnimationStart = time;
            skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[(int)lastMoveDirection], false).AnimationStart = time;
            //if (!(_player.CanAttack && _player.IsAttacking))
            //    skeletonAnimation.AnimationState.SetAnimation(0, MoveAnimations[(int)lastMoveDirection], false).AnimationStart = time;

            yield return new WaitForSeconds(fixedTime/2);
        }
    }

    void MoveAndIdle(float time)
    {
        if (_inputDirection == Vector2.zero)
        {
            skeletonAnimation.AnimationState.SetAnimation(1, IdleAnimations[(int)lastMoveDirection], false).AnimationStart = time;
            if (!(PlayerMain.Instance.canAttack && PlayerMain.Instance.isAttacking))
                skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimations[(int)lastMoveDirection], false).AnimationStart = time;
            return;
        }

        skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[(int)lastMoveDirection], false).AnimationStart = time;
        if (!(PlayerMain.Instance.canAttack && PlayerMain.Instance.isAttacking))
            skeletonAnimation.AnimationState.SetAnimation(0, MoveAnimations[(int)lastMoveDirection], false).AnimationStart = time;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            SetAnimations();
            Debug.LogWarning("X Pressed");
        }
    }

    public void SetAnimations(/*List<string> weaponIdle,List<string> leftAttack,List<string> charging, List<string> rightAttack*/)
    {
        leftAttackAnimations = PlayerMain.Instance.nowWeapon.leftAttackAnimations;
        Debug.LogWarning("excuted");
        WeaponIdleAnimations = PlayerMain.Instance.nowWeapon.WeaponIdleAnimations;
        chargingAttack = PlayerMain.Instance.nowWeapon.chargingAttack;
        rightAttackAnimations = PlayerMain.Instance.nowWeapon.rightAttackAnimations;
    }

    public void SetAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation, int track = 0, bool loop = true, float startTime = 0f)
    {
        animator.AnimationState.SetAnimation(track, animation, loop).Reverse = false;
        animator.AnimationState.SetAnimation(track, animation, loop).AnimationStart = startTime;
        
    }

    float reverseTime = 0;
    public void SetReverseAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation, int track = 0)
    {
        animator.AnimationState.SetAnimation(track, animation, true).Reverse = true;

    }

}
