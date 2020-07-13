using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimWhereFacingThenRotate : AimWhereFacing
	{
		// [MakeConfigurable]
		public Quaternion rotate;
		
		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			Bullet[] output = base.Shoot (spawner, bulletPrefab, positionOffset);
			spawner.up = spawner.up.Rotate(rotate);
			return output;
		}
	}
}