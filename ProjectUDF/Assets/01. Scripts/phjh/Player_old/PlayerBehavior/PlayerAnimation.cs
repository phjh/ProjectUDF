using Spine;
using Spine.Unity;
using System;
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

    #region SpineAnimations

    public List<AnimationReferenceAsset> IdleAnimations;

    public List<AnimationReferenceAsset> MoveAnimations;

    private List<AnimationReferenceAsset> WeaponIdleAnimations;

    [SpineAnimation]
    public List<string> DodgeAnimation;

    public AnimationReferenceAsset dieAnimation;

    #endregion

    public int aimAngle = 0;

    public Vector2 _inputDirection;
    
    float timescale;

    protected void Start()
    {
        skeletonAnimation = PlayerMain.Instance.skeletonAnimation;
        PlayerMain.Instance.inputReader.MovementEvent += SetMovement;
        PlayerMain.Instance.OnWeaponSetting += SetAnimations;
        PlayerMain.Instance.DeadEvent += OnDie;
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
        SpineAnimator.Instance.isEmpty = true;
        skeletonAnimation.timeScale = 0.5f;
        SpineAnimator.Instance.SetAnimation(skeletonAnimation, dieAnimation, 0);
        SpineAnimator.Instance.SetAnimation(skeletonAnimation, dieAnimation, 1);

        Debug.LogWarning(dieAnimation.name);
        Debug.LogWarning(nameof(SetDeadScene) + " andn " + dieAnimation.Animation.Duration);

        Invoke(nameof(SetDeadScene) , dieAnimation.Animation.Duration);
    }

    void SetDeadScene()
    {
        if (InGameSceneManager.Instance == null)
        {
            Debug.LogWarning("ingamescene is null");
            return;
        }
        InGameSceneManager.Instance.SetSceneName("GameResultScene");
    }

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
        lastMoveDirection = (MoveDirectionList)aimAngle;
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

            if ((_inputDirection == Vector2.zero || !PlayerMain.Instance.canMove) && !SpineAnimator.Instance.Skip)
            {
                SpineAnimator.Instance.SetSortedAnimation(skeletonAnimation, IdleAnimations, (int)lastMoveDirection, 1, startTime: time);
            }
            else
            {
                //여기서 왜 대체 그렇게 되는가ㅏㅏㅏㅏ
                if ((int)lastMoveDirection == 7)
                {
                    SpineAnimator.Instance.SetAnimation(skeletonAnimation, MoveAnimations[6], 1, startTime: time);
                }
                else
                    SpineAnimator.Instance.SetSortedAnimation(skeletonAnimation, MoveAnimations, (int)lastMoveDirection, 1, startTime: time);
            }

            if (WeaponIdleAnimations != null && WeaponIdleAnimations.Count != 0)
            {
                if ((int)lastMoveDirection == 7)
                    Debug.LogWarning($"dir : {(int)lastMoveDirection}, animation name : {WeaponIdleAnimations[(int)lastMoveDirection].Animation.Name}");
                SpineAnimator.Instance.SetAnimation(skeletonAnimation, WeaponIdleAnimations[(int)lastMoveDirection], 0, startTime: time % WeaponIdleAnimations[0].Animation.Duration);
            }
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
        {
            Debug.LogWarning("Playermain_nowWeapon Is null");
            return;
        }
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
