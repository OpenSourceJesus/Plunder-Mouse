using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimWhereFacingThenTargetPlayer : AimWhereFacing
	{
		// [MakeConfigurable]
		public float retargetTime;
		
		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			Bullet[] output = base.Shoot (spawner, bulletPrefab, positionOffset);
			foreach (Bullet bullet in output)
				bullet.StartCoroutine(RetargetAfterDelay (bullet, retargetTime));
			return output;
		}
		
		public override Vector3 GetRetargetDirection (Bullet bullet)
		{
			return PlayerObject.CurrentActive.trs.position - bullet.trs.position;
		}
	}
}