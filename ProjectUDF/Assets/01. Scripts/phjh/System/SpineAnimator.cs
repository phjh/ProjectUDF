using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpineAnimator : MonoSingleton<SpineAnimator>
{
    public bool isEmpty = false;

    public void SetAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation, int track = 0, bool loop = false, float startTime = 0f, float speed = 1f, bool reversed = false)
    {
        if (animation == null)
            return;

        TrackEntry tracks = new();
        tracks = animator.AnimationState.SetAnimation(track, animation, loop);
        tracks.Reverse = reversed;
        tracks.AnimationStart = startTime;
    }
    public void AddAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation,float delay, int track = 0, bool loop = false, float startTime = 0f, float speed = 1f, bool reversed = false)
    {
        if (animation == null)
            return;

        TrackEntry tracks = new();
        tracks = animator.AnimationState.AddAnimation(track, animation, loop, delay);
        tracks.Reverse = reversed;
        tracks.AnimationStart = startTime;
    }

    public void SetEmptyAnimation(SkeletonAnimation animator, int track = 0, float time = 0f)
    {
        isEmpty = true;
        animator.AnimationState.SetEmptyAnimation(track, time + 0.1f);
        Task.Run(async() =>
        {
            await Task.Delay(Mathf.RoundToInt(time * 1000));
            isEmpty = false;
        });
    }

    public void SetSortedAnimation(SkeletonAnimation animator, List<AnimationReferenceAsset> animations, int dir, int track = 0, bool loop = false, float startTime = 0f, float speed = 1f, bool reversed = false)
    {
        AnimationReferenceAsset animationAsset = GetSortedAnimation(animations, dir);

        SetAnimation(animator, animationAsset, track, loop, startTime, speed, reversed);
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
    public AnimationReferenceAsset GetSortedAnimation(List<AnimationReferenceAsset> animation, int dir)
    {
        if (dir < 0 || dir > 8) 
        {
            Debug.LogError(dir);
            return null;
        }
        return animation[arr[dir]];
    }
}
