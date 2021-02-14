using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class BulletPattern : ScriptableObject//, IConfigurable
	{
		public virtual void Init (Transform spawner)
		{			
		}

		public virtual Vector3 GetShootDirection (Transform spawner)
		{
			return spawner.forward;
		}
		
		public virtual Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			Vector3 direction = GetShootDirection(spawner);
			Bullet bullet = ObjectPool.Instance.SpawnComponent<Bullet>(bulletPrefab, spawner.position + direction * positionOffset, Quaternion.LookRotation(direction));
			// if (bullet == default(Bullet))
			// 	return null;
			return new Bullet[] { bullet };
		}
		
		public virtual Bullet[] Shoot (Vector3 spawnPos, Vector3 direction, Bullet bulletPrefab, float positionOffset = 0)
		{
			Bullet bullet = ObjectPool.Instance.SpawnComponent<Bullet>(bulletPrefab, spawnPos + direction.normalized * positionOffset, Quaternion.LookRotation(direction));
			// if (bullet == default(Bullet))
			// 	return null;
			return new Bullet[] { bullet };
		}
		
		public virtual IEnumerator RetargetAfterDelay (Bullet bullet, float delay)
		{
			yield return new WaitForSeconds(delay);
			// if (!bullet.gameObject.activeSelf)
			// 	yield break;
			yield return Retarget (bullet);
		}
		
		public virtual IEnumerator RetargetAfterDelay (Bullet bullet, Vector3 direction, float delay)
		{
			yield return new WaitForSeconds(delay);
			// if (!bullet.gameObject.activeSelf)
			// 	yield break;
			yield return Retarget (bullet, direction);
		}

		public virtual Bullet Retarget (Bullet bullet)
		{
			bullet.trs.forward = GetRetargetDirection(bullet);
			bullet.rigid.velocity = bullet.trs.forward * bullet.moveSpeed;
			return bullet;
		}
		
		public virtual Bullet Retarget (Bullet bullet, Vector3 direction)
		{
			bullet.trs.forward = direction;
			bullet.rigid.velocity = bullet.trs.forward * bullet.moveSpeed;
			return bullet;
		}
		
		public virtual Vector3 GetRetargetDirection (Bullet bullet)
		{
			return bullet.trs.forward;
		}
		
		public virtual IEnumerator SplitAfterDelay (Bullet bullet, Bullet splitBulletPrefab, float delay, float positionOffset = 0)
		{
			yield return new WaitForSeconds(delay);
			// if (!bullet.gameObject.activeSelf)
			// 	yield break;
			yield return Split (bullet, splitBulletPrefab, positionOffset);
		}
		
		public virtual IEnumerator SplitAfterDelay (Bullet bullet, Vector3 direction, Bullet splitBulletPrefab, float delay, float positionOffset = 0)
		{
			yield return new WaitForSeconds(delay);
			// if (!bullet.gameObject.activeSelf)
			// 	yield break;
			yield return Split (bullet, direction, splitBulletPrefab, positionOffset);
		}

		public virtual Bullet[] Split (Bullet bullet, Bullet splitBulletPrefab, float positionOffset = 0)
		{
			return Shoot (bullet.trs.position, GetSplitDirection(bullet), splitBulletPrefab, positionOffset);
		}

		public virtual Bullet[] Split (Bullet bullet, Vector3 direction, Bullet splitBulletPrefab, float positionOffset = 0)
		{
			return Shoot (bullet.trs.position, direction, splitBulletPrefab, positionOffset);
		}
		
		public virtual Vector3 GetSplitDirection (Bullet bullet)
		{
			return bullet.trs.forward;
		}
	}
}