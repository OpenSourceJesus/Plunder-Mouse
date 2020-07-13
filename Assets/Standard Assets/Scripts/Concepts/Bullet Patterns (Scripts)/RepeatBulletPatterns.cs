using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class RepeatBulletPatterns : BulletPattern
	{
		// [MakeConfigurable]
		public int repeatCount;
		public BulletPattern[] bulletPatterns;

		public override void Init (Transform spawner)
		{
			base.Init (spawner);
			foreach (BulletPattern bulletPattern in bulletPatterns)
				bulletPattern.Init (spawner);
		}

		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			List<Bullet> output = new List<Bullet>();
			Bullet[] bullets;
			for (int i = 0; i < repeatCount; i ++)
			{
				foreach (BulletPattern bulletPattern in bulletPatterns)
				{
					bullets = bulletPattern.Shoot (spawner, bulletPrefab, positionOffset);
					// if (bullets == null)
					// 	return null;
					// else
						output.AddRange(bullets);
				}
			}
			return output.ToArray();
		}
		
		public override Bullet[] Shoot (Vector3 spawnPos, Vector3 direction, Bullet bulletPrefab, float positionOffset = 0)
		{
			List<Bullet> output = new List<Bullet>();
			Bullet[] bullets;
			for (int i = 0; i < repeatCount; i ++)
			{
				foreach (BulletPattern bulletPattern in bulletPatterns)
				{
					bullets = bulletPattern.Shoot (spawnPos, direction, bulletPrefab, positionOffset);
					// if (bullets == null)
					// 	return null;
					// else
						output.AddRange(bullets);
				}
			}
			return output.ToArray();
		}
	}
}