using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class ShootInLocalDirectionThenContinuouslySplitInRelativeArc : AimInLocalDirection
	{
		public Bullet splitBulletPrefab;
		public Vector3 rotationToSplitArc;
		public Vector3 splitArcRotaAxis;
		public float splitDelay;
		public float splitArcDegrees;
		public int splitNumber;
		public Transform splitDirectionTrs;
		
		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			Bullet[] output = base.Shoot (spawner, bulletPrefab, positionOffset);
			foreach (Bullet bullet in output)
				bullet.StartCoroutine(SplitAfterDelay (bullet, splitBulletPrefab, splitDelay, positionOffset));
			return output;
		}
		
		public override Bullet[] Split (Bullet bullet, Bullet splitBulletPrefab, float positionOffset = 0)
		{
			Bullet[] output = new Bullet[splitNumber];
			Vector3 toPlayer = PlayerObject.CurrentActive.trs.position - bullet.trs.position;
			splitDirectionTrs.rotation = bullet.trs.rotation;
			splitDirectionTrs.Rotate(rotationToSplitArc);
			for (int i = 0; i < splitNumber; i ++)
			{
				splitDirectionTrs.Rotate(splitArcRotaAxis.normalized * splitArcDegrees / splitNumber);
				// bullets = base.Split (bullet, splitDirectionTrs.forward, splitBulletPrefab, positionOffset);
				output[i] = GameManager.GetSingleton<ObjectPool>().SpawnComponent<Bullet>(splitBulletPrefab, bullet.trs.position + splitDirectionTrs.forward.normalized * positionOffset, splitDirectionTrs.rotation);
			}
			return output;
		}

		public virtual Quaternion GetSplitArcRota (float splitAngle)
		{
			return Quaternion.Euler(splitArcRotaAxis.normalized * splitAngle);
		}
		
		public override IEnumerator SplitAfterDelay (Bullet bullet, Bullet splitBulletPrefab, float delay, float positionOffset = 0)
		{
			while (true)
			{
				yield return new WaitForSeconds(delay);
				Split (bullet, splitBulletPrefab, positionOffset);
			}
		}
	}
}