using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class OrlobEnemy : ShootingEnemy
	{
		public AttackEntryDontCollideWithAttacker[] attackEntriesDontCollideWithAttacker;

		public override void OnAttackAnimationChanged (int animationFrameIndex)
		{
			foreach (AttackEntry attackEntry in attackEntriesDontCollideWithAttacker)
			{
				if (attackEntry.attackOnAnimationFrameIndex == animationFrameIndex)
					attackEntry.Attack ();
			}
		}
	}
}