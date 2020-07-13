using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class ShootBulletPatternThenRotate : BulletPattern
	{
		public BulletPattern bulletPattern;
		public Vector3 rotation;
		
		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			Bullet[] output = bulletPattern.Shoot (spawner, bulletPrefab, positionOffset);
			spawner.Rotate(rotation);
			return output;
		}
	}
}