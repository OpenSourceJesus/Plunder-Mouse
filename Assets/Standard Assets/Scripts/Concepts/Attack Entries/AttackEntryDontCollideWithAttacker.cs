using UnityEngine;
using System;
using System.Collections;

namespace PlunderMouse
{
	[Serializable]
	public class AttackEntryDontCollideWithAttacker : AttackEntry
	{
		public Enemy attacker;

		public override void Attack ()
		{
			Bullet[] bullets = bulletPattern.Shoot(spawner, bulletPrefab);
			foreach (Bullet bullet in bullets)
			{
				foreach (Collider collider in attacker.colliders)
				   Physics.IgnoreCollision(bullet.collider, collider, true);
				bullet.collider.enabled = true;
			}
		}
	}
}