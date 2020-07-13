using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Extensions;

namespace PlunderMouse
{
	public class ShootingEnemy : Enemy
	{
		public AttackEntry[] attackEntries;

		public override void Awake ()
		{
			base.Awake ();
			for (int i = 0; i < attackEntries.Length; i ++)
			{
				if (attackEntries[i].attackOnAnimationFrameIndex == -1)
				{
					attackEntries = attackEntries.RemoveAt(i);
					i --;
				}
			}
		}
		
		public override void OnEnable ()
		{
			base.OnEnable ();
			anim.animationDict[attackAnimName].onFrameChanged += OnAttackAnimationChanged;
		}
		
		public override void OnDisable ()
		{
			base.OnDisable ();
			anim.animationDict[attackAnimName].onFrameChanged -= OnAttackAnimationChanged;
		}
		
		public override void DoUpdate ()
		{
			// if (!awakened || dead)
			// 	return;
			base.DoUpdate ();
			HandleAttacking ();
		}
		
		public virtual void HandleAttacking ()
		{
			if (anim != null && !anim.GetCurrentlyPlayingAnimationNames().Contains(attackAnimName))
			{
				anim.StopAll (true);
				anim.Play (attackAnimName);
			}
			if (anim2 != null && !anim2.GetCurrentlyPlayingAnimationNames().Contains(attackAnimName))
			{
				anim2.StopAll ();
				anim2.Play (attackAnimName);
			}
		}
		
		public virtual void OnAttackAnimationChanged (int animationFrameIndex)
		{
			foreach (AttackEntry attackEntry in attackEntries)
			{
				if (attackEntry.attackOnAnimationFrameIndex == animationFrameIndex)
					attackEntry.Attack ();
			}
		}
	}
}