using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class ShootAtPlayerWithOffsetThenDespawnAndSplitInArcAimedTowardsPlayerWithOffset : AimAtPlayerXZWithOffset
	{
		// [MakeConfigurable]
		public float splitOffset;
		public Bullet splitBulletPrefab;
		// [MakeConfigurable]
		public float splitDelay;
		// [MakeConfigurable]
		public float splitArc;
		// [MakeConfigurable]
		public float splitNumber;
		
		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			Bullet[] output = base.Shoot (spawner, bulletPrefab, positionOffset);
			foreach (Bullet bullet in output)
				bullet.StartCoroutine(SplitAfterDelay (bullet, splitBulletPrefab, splitDelay, positionOffset));
			return output;
		}
		
		public override Bullet[] Split (Bullet bullet, Vector3 direction, Bullet splitBulletPrefab, float positionOffset = 0)
		{
			Bullet[] output = new Bullet[0];
			float toPlayer = (PlayerObject.CurrentActive.trs.position - bullet.trs.position).GetFacingAngle();
			for (float splitAngle = toPlayer - splitArc / 2 + splitOffset; splitAngle < toPlayer + splitArc / 2 + splitOffset; splitAngle += splitArc / splitNumber)
				output = base.Split (bullet, VectorExtensions.FromFacingAngle(splitAngle), splitBulletPrefab, positionOffset);
			// ObjectPool.Instance.Despawn (bullet.prefabIndex, bullet.gameObject, bullet.trs);
			Destroy(bullet.gameObject);
			return output;
		}
	}
}