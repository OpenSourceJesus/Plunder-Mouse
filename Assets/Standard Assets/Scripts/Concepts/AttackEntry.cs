using UnityEngine;
using System;
using System.Collections;

namespace PlunderMouse
{
	[Serializable]
	public class AttackEntry
	{
		public BulletPattern bulletPattern;
		public int attackOnAnimationFrameIndex;
		public Bullet bulletPrefab;
		public Transform spawner;
		
		public virtual void Attack ()
		{
			bulletPattern.Shoot (spawner, bulletPrefab);
		}
	}
}