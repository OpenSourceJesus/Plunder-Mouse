using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class ShootAtPlayerWithRandomOffsetAndGravityAndBulletSpeedBasedOnDistance : BulletPattern
	{
		public AnimationCurve bulletSpeedOverDistance;
		public float maxOffset;

		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			Vector3 toPlayer = PlayerObject.CurrentActive.trs.position + (Random.onUnitSphere * Random.value * maxOffset) - spawner.position;
			float x = toPlayer.SetY(0).magnitude;
			float y = -toPlayer.y;
			float speed = Mathf.Clamp(bulletSpeedOverDistance.Evaluate(toPlayer.magnitude), 0, bulletPrefab.moveSpeed);
			float speedSqr = speed * speed;
			float gravity = Physics.gravity.y;
			float parentheses = gravity * x * x + 2 * y * speedSqr;
			float sqrt = Mathf.Sqrt(speedSqr * speedSqr - gravity * parentheses);
			float denominator = gravity * x;
			float angle1 = Mathf.Atan((speedSqr + sqrt) / denominator) * Mathf.Rad2Deg;
			float angle2 = Mathf.Atan((speedSqr - sqrt) / denominator) * Mathf.Rad2Deg;
			Quaternion localRotation = spawner.localRotation;
			spawner.forward = Quaternion.Euler(Vector3.up * spawner.localEulerAngles.y) * toPlayer.GetXZ();
			spawner.localEulerAngles = spawner.localEulerAngles.SetX(Mathf.Max(angle1, angle2));
			Bullet bullet = ObjectPool.Instance.SpawnComponent<Bullet>(bulletPrefab, spawner.position + spawner.forward * positionOffset, spawner.rotation);
			spawner.localRotation = localRotation;
			bullet.moveSpeed = speed;
			bullet.rigid.velocity = bullet.trs.forward * speed;
			return new Bullet[] { bullet };
		}
	}
}