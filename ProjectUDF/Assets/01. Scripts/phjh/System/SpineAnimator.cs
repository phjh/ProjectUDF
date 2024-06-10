using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAnimator : MonoSingleton<SpineAnimator>
{

    public void SetAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation, int track = 0, bool loop = true, float startTime = 0f, float speed = 1f)
    {
        if (animation == null)
            return;

        animator.AnimationState.SetAnimation(track, animation, loop).Reverse = false;
        animator.AnimationState.SetAnimation(track, animation, loop).TimeScale = speed;
        animator.AnimationState.SetAnimation(track, animation, loop).AnimationStart = startTime;

    }

    public void SetSortedAnimation(SkeletonAnimation animator, List<AnimationReferenceAsset> animations, MoveDirectionList dir, int track = 0, bool loop = true, float startTime = 0f, float speed = 1f)
    {
        AnimationReferenceAsset animationAsset = GetSortedAnimation(animations, dir);

        SetAnimation(animator, animationAsset, track, loop, startTime, speed);
    }

    public void SetReverseAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation, int track = 0)
    {
        animator.AnimationState.SetAnimation(track, animation, true).Reverse = true;
    }

    public void SetAnimationAsEmpty(SkeletonAnimation animator, int track)
    {
        animator.AnimationState.SetEmptyAnimation(track, 0);
    }

    int[] arr = { 1, 7, 3, 5, 0, 4, 2, 6 };
    public AnimationReferenceAsset GetSortedAnimation(List<AnimationReferenceAsset> animation, MoveDirectionList dir)
    {
        if ((int)dir < 0 || (int)dir > 8) 
        {
            Debug.LogError((int)dir);
            Debug.LogError(dir);
            return null;
        }
        return animation[arr[(int)dir]];
    }
}
