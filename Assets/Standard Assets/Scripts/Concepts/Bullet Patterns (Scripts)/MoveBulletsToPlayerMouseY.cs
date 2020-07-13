using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class MoveBulletsToPlayerMouseY : BulletPattern
	{
		public BulletPattern[] bulletPatterns;
		
		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			List<Bullet> output = new List<Bullet>();
			foreach (BulletPattern bulletPattern in bulletPatterns)
				output.AddRange(bulletPattern.Shoot (spawner, bulletPrefab, positionOffset));
			GameManager.GetSingleton<GameManager>().StartCoroutine(MoveRoutine (output));
			return output.ToArray();
		}
		
		public override Bullet[] Shoot (Vector3 spawnPos, Vector3 direction, Bullet bulletPrefab, float positionOffset = 0)
		{
			List<Bullet> output = new List<Bullet>();
			foreach (BulletPattern bulletPattern in bulletPatterns)
				output.AddRange(bulletPattern.Shoot (spawnPos, direction, bulletPrefab, positionOffset));
			GameManager.GetSingleton<GameManager>().StartCoroutine(MoveRoutine (output));
			return output.ToArray();
		}

		public virtual IEnumerator MoveRoutine (List<Bullet> bullets)
		{
			List<BulletEntry> bulletEntries = new List<BulletEntry>();
			foreach (Bullet bullet in bullets)
			{
				BulletEntry bulletEntry = new BulletEntry(bullet);
				if (bulletEntry.yMoveDirection != 0)
				{
					Retarget (bullet, Vector3.up * bulletEntry.yMoveDirection);
					bulletEntries.Add(bulletEntry);
				}
			}
			while (bulletEntries.Count > 0)
			{
                for (int i = 0; i < bulletEntries.Count; i++)
				{
                    BulletEntry bulletEntry = (BulletEntry) bulletEntries[i];
                    if (MathfExtensions.Sign(GameManager.GetSingleton<PlayerMouse>().trs.position.y - bulletEntry.bullet.trs.position.y) != bulletEntry.yMoveDirection)
					{
						bulletEntries.RemoveAt(i);
						i --;
					}
				}
				yield return new WaitForEndOfFrame();
			}
			foreach (Bullet bullet in bullets)
				Retarget (bullet);
		}

		public override Bullet Retarget (Bullet bullet)
		{
			bullet.rigid.velocity = GetRetargetDirection(bullet) * bullet.moveSpeed;
			return bullet;
		}
		
		public override Bullet Retarget (Bullet bullet, Vector3 direction)
		{
			bullet.rigid.velocity = direction * bullet.moveSpeed;
			return bullet;
		}

		public class BulletEntry
		{
			public Bullet bullet;
			public int yMoveDirection;

			public BulletEntry (Bullet bullet)
			{
				this.bullet = bullet;
				yMoveDirection = MathfExtensions.Sign(GameManager.GetSingleton<PlayerMouse>().trs.position.y - bullet.trs.position.y);
			}
		}
	}
}