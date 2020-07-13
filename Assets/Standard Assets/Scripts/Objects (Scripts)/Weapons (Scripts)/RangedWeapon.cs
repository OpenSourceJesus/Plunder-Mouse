using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class RangedWeapon : Weapon, IUpdatable
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
		public Transform spawnTrs;
		public BulletPattern bulletPattern;
		public Bullet bulletPrefab;
		public Timer reloadTimer;
		bool isLoaded = true;
		public LineRenderer aimingVisualizer;
		public float aimingVisualizerDeltaTime;

		public virtual void OnEnable ()
		{
			reloadTimer.onFinished += Reload;
			if (aimingVisualizer != null)
				GameManager.updatables = GameManager.updatables.Add(this);
		}

		public override void Attack ()
		{
			if (!isLoaded)
				return;
			isLoaded = false;
			base.Attack ();
			StartCoroutine(AttackRoutine ());
			reloadTimer.Reset ();
			reloadTimer.Start ();
		}

		public virtual void Reload (params object[] args)
		{
			isLoaded = true;
		}

		public virtual IEnumerator AttackRoutine ()
		{
			Bullet[] bullets;
			do
			{
				bullets = bulletPattern.Shoot(spawnTrs, bulletPrefab);
				yield return new WaitForEndOfFrame();
			} while (bullets == null);
		}

		public virtual void DoUpdate ()
		{
			Vector3[] points = new Vector3[aimingVisualizer.positionCount];
			int currentPointIndex = 0;
			Vector3 currentPosition = spawnTrs.position;
			Vector3 currentVelocity = spawnTrs.forward * bulletPrefab.moveSpeed;
			float currentTravelRange = 0;
			do
			{
				currentVelocity += Physics.gravity * aimingVisualizerDeltaTime;
				currentVelocity *= 1 - aimingVisualizerDeltaTime * bulletPrefab.rigid.drag;
				currentPosition += currentVelocity * aimingVisualizerDeltaTime;
				currentTravelRange += currentVelocity.magnitude;
				if (currentTravelRange >= bulletPrefab.range / (points.Length - 1) * currentPointIndex)
				{
					points[currentPointIndex] = currentPosition;
					currentPointIndex ++;
					if (currentPointIndex == points.Length)
						break;
				}
			} while (true);
			aimingVisualizer.SetPositions(points);
		}

		public virtual void OnDisable ()
		{
			reloadTimer.onFinished -= Reload;
			if (aimingVisualizer != null)
				GameManager.updatables = GameManager.updatables.Remove(this);
		}
	}
}