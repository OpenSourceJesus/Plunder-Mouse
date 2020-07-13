using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class EnemyGroup : MonoBehaviour, IUpdatable
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
#if UNITY_EDITOR
		public bool useCustomYPosition;
		public float yPosition;
#endif
		public Transform trs;
		public Enemy[] enemies;
		float patrolRange;
		Vector3 destination;
		public float stopRange;
		float stopRangeSqr;
		public bool patrolLand;
		public bool patrolWater;
		Vector3 toDestination;
		public LayerMask whatIsLand;
		public LayerMask whatIsWater;

		public virtual void Awake ()
		{
			float patrolArea = 0;
			foreach (Enemy enemy in enemies)
				patrolArea += new Circle2D(enemy.patrolNonflying.patrolRange).Area - new Circle2D(enemy.patrolNonflying.trs.lossyScale.x * enemy.patrolNonflying.controller.radius).Area;
			patrolRange = (float) Mathf.Sqrt(patrolArea / Mathf.PI);
			stopRangeSqr = stopRange * stopRange;
		}

		public virtual void OnEnable ()
		{
			foreach (Enemy enemy in enemies)
				enemy.patrolNonflying.enabled = false;
			SetDestination ();
			GameManager.updatables = GameManager.updatables.Add(this);
		}

		public virtual void DoUpdate ()
		{
			foreach (Enemy enemy in enemies)
			{
				toDestination = (destination - enemy.patrolNonflying.trs.position).SetY(0);
				enemy.patrolNonflying.move = Vector3.ClampMagnitude(toDestination, 1);
				enemy.patrolNonflying.move *= enemy.patrolNonflying.moveSpeed;
				enemy.patrolNonflying.trs.forward = enemy.patrolNonflying.move;
				enemy.patrolNonflying.HandleGravity ();
				if (enemy.patrolNonflying.controller != null && enemy.patrolNonflying.controller.enabled)
					enemy.patrolNonflying.controller.Move(enemy.patrolNonflying.move * Time.deltaTime);
				else
					enemy.patrolNonflying.rigid.velocity = enemy.patrolNonflying.move * enemy.patrolNonflying.multiplyRigidMoveSpeed;
				if (toDestination.sqrMagnitude <= stopRangeSqr || enemy.patrolNonflying.controller.collisionFlags.ToString().Contains("Sides"))
					SetDestination ();
			}
		}

		public virtual void SetDestination ()
		{
			do
			{
				destination = trs.position + (Random.insideUnitCircle * patrolRange).XYToXZ();
				RaycastHit hit;
				if (Physics.Raycast(destination.SetY(trs.position.y), Vector3.down, out hit, Mathf.Infinity, whatIsLand.AddToMask(whatIsWater)))
				{
					bool shouldReturn = true;
					if (!patrolLand && LayerMaskExtensions.MaskContainsLayer(whatIsLand, hit.collider.gameObject.layer))
						shouldReturn = false;
					else if (!patrolWater && LayerMaskExtensions.MaskContainsLayer(whatIsWater, hit.collider.gameObject.layer))
						shouldReturn = false;
					if (shouldReturn)
						return;
				}
			} while (true);
		}

		public virtual void OnDisable ()
		{
			GameManager.updatables = GameManager.updatables.Remove(this);
		}
	}
}