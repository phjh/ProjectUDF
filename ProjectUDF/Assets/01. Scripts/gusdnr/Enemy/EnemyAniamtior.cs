using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAniamtior : MonoBehaviour
{
	public EnemyMain EM { get; set; }

	public void InitAnimator (EnemyMain em)
	{
		EM = em;
		ConnectedVisual = false;

		if (EAnimator == null)
			EAnimator = transform.Find("Visual").GetComponent<SkeletonAnimation>();
		
		if(EAnimator != null) ConnectedVisual = true;
	}

    public SkeletonAnimation EAnimator;
	private bool ConnectedVisual = false;

	[SpineAnimation] public string IdleAnimation;
	[SpineAnimation] public string MoveAnimation;
	[SpineAnimation] public string AttackAnimation;
	[SpineAnimation] public string CooldownAnimation;
	[SpineAnimation] public string DieAnimation;

	private IEnumerator SetStateAnimation()
	{
		if (ConnectedVisual == false) yield return ConnectedVisual;

	}

	public void OnDie()
	{
		EAnimator.timeScale = 1f;
		EAnimator.AnimationState.SetAnimation(0, DieAnimation, false);
	}
}
