using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class CatEnemy : ShootingEnemy
	{
		public float despawnRangeAfterDeath;
		public int currentAttackEntryIndex;
		
		public override void OnEnable ()
		{
			base.OnEnable ();
			rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
			rigid.useGravity = false;
		}
		
		public override void OnAttackAnimationChanged (int animationFrameIndex)
		{
			AttackEntry currentAttackEntry = attackEntries[currentAttackEntryIndex];
			if (currentAttackEntry.attackOnAnimationFrameIndex == animationFrameIndex)
			{
				currentAttackEntry.Attack ();
				currentAttackEntryIndex ++;
				if (currentAttackEntryIndex >= attackEntries.Length)
					currentAttackEntryIndex = 0;
			}
		}
		
		public override void Death ()
		{
			base.Death ();
			rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			rigid.useGravity = true;
			ObjectPool.Instance.RangeDespawn (prefabIndex, gameObject, trs, despawnRangeAfterDeath);
		}
	}
}