using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimation : MonoBehaviour
{
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

	SkeletonAnimation skeletonAnimation;

    MoveDirectionList lastMoveDirection;

    [SerializeField]
    PlayerAim aim;

    Coroutine animationCoroutine;

    #region SpineAnimations

    public List<AnimationReferenceAsset> IdleAnimations;

    public List<AnimationReferenceAsset> MoveAnimations;

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
        skeletonAnimation = PlayerMain.Instance.skeletonAnimation;
        PlayerMain.Instance.inputReader.MovementEvent += SetMovement;
        PlayerMain.Instance.OnWeaponSetting += SetAnimations;
        PlayerStats.OnDeadPlayer += OnDie;
        timescale = skeletonAnimation.timeScale;
        SetAnimations();
        StartCoroutine(MoveAndIdle());
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

    //원래 플레이어 애니메이션 코루틴
    //IEnumerator SetAnimation()
    //{
    //    float time = 0;
    //    float fixedTime = 0.01f;
    //    while (true)
    //    {
    //        //애니메이션 재생/역재생용
    //        if(time > 0.8f)
    //        {
    //            time = 0;
    //        }
    //        else
    //        {
    //            time += fixedTime;
    //        }

    //        skeletonAnimation.AnimationState.TimeScale = PlayerMain.Instance.stat.MoveSpeed.GetValue() / 3f;

    //        //회피 애니메이션
    //        if (PlayerMain.Instance.isDodging)
    //        {
    //            skeletonAnimation.AnimationState.SetAnimation(0, DodgeAnimation[(int)lastMoveDirection], false);
    //            skeletonAnimation.AnimationState.SetAnimation(1, DodgeAnimation[(int)lastMoveDirection], false);
    //            yield return new WaitForSeconds(0.5f);
    //        }

    //        if (Input.GetMouseButton(0) && (PlayerMain.Instance.preparingAttack||PlayerMain.Instance.canAttack))
    //        {
    //            isLeftPressed = true;
    //            aimAngle = aim.Angle;
    //            if (_inputDirection == Vector2.zero)
    //            {
    //                skeletonAnimation.AnimationState.SetAnimation(1, IdleAnimations[aimAngle], false).AnimationStart = time;
    //            }
    //            else
    //            {
    //                int attackingdir = Mathf.Abs((int)lastMoveDirection - aimAngle);
    //                if (attackingdir >= 3 && attackingdir <= 5)
    //                {
    //                    skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = 0.8f - time;
    //                }
    //                else
    //                {
    //                    skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = time;
    //                }
    //            }
    //            if (additionalAnimations.Count > 1)
    //                skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
    //            skeletonAnimation.AnimationState.SetAnimation(0, chargingAttack[aimAngle], true).AnimationStart = time;
    //            yield return new WaitForSeconds(fixedTime);
    //            continue;
    //        }
    //        else if(isLeftPressed)
    //        {
    //            skeletonAnimation.AnimationState.TimeScale = 1.2f;
    //            isLeftPressed = false;
    //            lastMoveDirection = (MoveDirectionList)aimAngle;
    //            skeletonAnimation.AnimationState.SetAnimation(0, leftAttackAnimations[aimAngle], false).AnimationStart = 0.25f;
    //            skeletonAnimation.AnimationState.SetAnimation(1, leftAttackAnimations[aimAngle], false).AnimationStart = 0.25f;
    //            skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0, 0);
    //            yield return new WaitForSeconds(0.5f);
    //        }

    //        if (Input.GetMouseButton(1) && PlayerMain.Instance.canAttack)
    //        {
    //            aimAngle = aim.Angle;
    //            if (_inputDirection == Vector2.zero)
    //                skeletonAnimation.AnimationState.SetAnimation(1, IdleAnimations[aimAngle], false).AnimationStart = time;
    //            else
    //            {
    //                int attackingdir = Mathf.Abs((int)lastMoveDirection - aimAngle);
    //                if (attackingdir >= 3 && attackingdir <= 5)
    //                    skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = 0.8f - time;
    //                else
    //                    skeletonAnimation.AnimationState.SetAnimation(1, MoveAnimations[aimAngle], false).AnimationStart = time;
    //            }
    //            if (additionalAnimations.Count > 1)
    //                skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
    //            skeletonAnimation.AnimationState.SetAnimation(0, WeaponIdleAnimations[aimAngle], false).AnimationStart = time;
    //            if(additionalAnimations.Count != 0 && !isRightPressed)
    //            {
    //                //SetAnimation(skeletonAnimation, additionalAnimations[0], 0, false, 0);
    //                PlayerMain.Instance.canMove = false;
    //                yield return new WaitForSeconds(additionalAnimations[0].Animation.Duration/2);
    //            }
    //            PlayerMain.Instance.canMove = true;
    //            isRightPressed = true;

    //            yield return new WaitForSeconds(fixedTime);
    //            continue;
    //        }
    //        else if(isRightPressed)
    //        {
    //            SpineAnimator.Instance.SetAnimation(skeletonAnimation, rightAttackAnimations[aimAngle], 1);
    //            skeletonAnimation.AnimationState.SetAnimation(0, rightAttackAnimations[aimAngle], false);
    //            skeletonAnimation.AnimationState.SetAnimation(1, rightAttackAnimations[aimAngle], false);
    //            if (additionalAnimations.Count > 1)
    //            {
    //                //SetAnimation(skeletonAnimation, additionalAnimations[1], 0, false, 0);
    //                PlayerMain.Instance.canMove = false;
    //                yield return new WaitForSeconds(additionalAnimations[1].Animation.Duration/2);
    //            }
    //            PlayerMain.Instance.canMove = true;
    //            yield return new WaitForSeconds(rightAttackAnimations[0].Animation.Duration + 0.1f);
    //            lastMoveDirection = (MoveDirectionList)aimAngle;
    //            isRightPressed = false;
    //        }

    //        yield return new WaitForSeconds(fixedTime/2);
    //    }
    //} //원래 플레이어 애니메이션

    void SetDirection()
    {
        bool isRight = _inputDirection.x > 0;
        bool isUp = _inputDirection.y > 0;

        if (_inputDirection == Vector2.zero)
        {
            //다른곳 바라보지 않게 막는용
        }
        else if (Mathf.Abs(_inputDirection.x) > Mathf.Abs(_inputDirection.y))
        {
            lastMoveDirection = isRight ? MoveDirectionList.Right : MoveDirectionList.Left;
        }
        else if (Mathf.Abs(_inputDirection.x) < Mathf.Abs(_inputDirection.y))
        {
            lastMoveDirection = isUp ? MoveDirectionList.Back : MoveDirectionList.Front;
        }
        else if (isRight)
        {
            lastMoveDirection = isUp ? MoveDirectionList.RightBack : MoveDirectionList.Rightfront;
        }
        else if (!isRight)
        {
            lastMoveDirection = isUp ? MoveDirectionList.LeftBack : MoveDirectionList.Leftfront;
        }
    }   //플레이어 방향 계산

    void OnAttackMoving(float time)
    {
        aimAngle = aim.Angle;
        if (_inputDirection == Vector2.zero)
        {
            SpineAnimator.Instance.SetSortedAnimation(skeletonAnimation, IdleAnimations, aimAngle, 1, startTime: time);
        }
        else
        {
            int attackingdir = Mathf.Abs((int)lastMoveDirection - aimAngle);
            if (attackingdir >= 3 && attackingdir <= 5)
            {
                SpineAnimator.Instance.SetSortedAnimation(skeletonAnimation, MoveAnimations, aimAngle, 1, startTime: MoveAnimations[0].Animation.Duration - time, reversed: true);
            }
            else
            {
                SpineAnimator.Instance.SetSortedAnimation(skeletonAnimation, MoveAnimations, aimAngle, 1, startTime: time);
            }
        }
        lastMoveDirection = (MoveDirectionList)aimAngle;
    }

    IEnumerator MoveAndIdle()   //움직임 애니메이션
    {
        float time = 0;

        while (true)
        {
            if (SpineAnimator.Instance.isEmpty)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            time += Time.deltaTime;

            if (time > MoveAnimations[0].Animation.Duration)
                time -= MoveAnimations[0].Animation.Duration;

            if (PlayerMain.Instance.isDodging)
            {
                time = 0;
                skeletonAnimation.AnimationState.SetAnimation(0, DodgeAnimation[(int)lastMoveDirection], false);
                skeletonAnimation.AnimationState.SetAnimation(1, DodgeAnimation[(int)lastMoveDirection], false);
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            if (PlayerMain.Instance.preparingAttack || PlayerMain.Instance.isAttacking)
            {
                OnAttackMoving(time);
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }

            SetDirection();

            if (_inputDirection == Vector2.zero || !PlayerMain.Instance.canMove)
            {
                SpineAnimator.Instance.SetSortedAnimation(skeletonAnimation, IdleAnimations, (int)lastMoveDirection, 1, startTime: time);
            }
            else
            {
                SpineAnimator.Instance.SetSortedAnimation(skeletonAnimation, MoveAnimations, (int)lastMoveDirection, 1, startTime: time);
            }

            if (WeaponIdleAnimations != null && WeaponIdleAnimations.Count != 0)
                SpineAnimator.Instance.SetAnimation(skeletonAnimation, WeaponIdleAnimations[(int)lastMoveDirection], startTime: time);
            else
                Debug.Log("no WeaponIdle Animations");

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void Update()
    {
        MoveAndIdle();
        if(Input.GetKeyDown(KeyCode.X))
        {
            SetAnimations();
            Debug.LogWarning("X Pressed");
        }
    }


    public void SetAnimations()
    {
        if (PlayerMain.Instance.nowWeapon == null)
            Debug.LogWarning("Playermain_nowWeapon Is null");
        WeaponIdleAnimations = GetSortedAnimationList(PlayerMain.Instance.nowWeapon.WeaponIdleAnimations);
    }

    int[] arr = { 1, 7, 3, 5, 0, 4, 2, 6 };
    List<AnimationReferenceAsset> GetSortedAnimationList(List<AnimationReferenceAsset> animation)
    {
        List<AnimationReferenceAsset> list = new();
        foreach(int i in arr)
            list.Add(animation[i]);
        return list;
    }

}
