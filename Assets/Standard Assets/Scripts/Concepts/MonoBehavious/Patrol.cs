using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class Patrol : MoveableEntity, IUpdatable
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
		public float patrolRange;
		Vector3 initPosition;
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
			initPosition = trs.position;
			stopRangeSqr = stopRange * stopRange;
		}

		public virtual void OnEnable ()
		{
			SetDestination ();
			GameManager.updatables = GameManager.updatables.Add(this);
		}

		public virtual void DoUpdate ()
		{
			toDestination = destination - trs.position;
			if (rigid.useGravity)
				toDestination = toDestination.SetY(0);
			move = Vector3.ClampMagnitude(toDestination, 1);
			move *= moveSpeed;
			trs.forward = move;
			HandleGravity ();
			if (controller != null && controller.enabled)
				controller.Move(move * Time.deltaTime);
			else
				rigid.velocity = move * multiplyRigidMoveSpeed;
			if (toDestination.sqrMagnitude <= stopRangeSqr || controller.collisionFlags.ToString().Contains("Sides"))
				SetDestination ();
		}

		public virtual void SetDestination ()
		{
			if (!rigid.useGravity)
			{
				destination = initPosition + Random.onUnitSphere * Random.value * patrolRange;
				return;
			}
			do
			{
				destination = initPosition + (Random.insideUnitCircle * patrolRange).XYToXZ();
				RaycastHit hit;
				if (Physics.Raycast(destination.SetY(trs.position.y + patrolRange), Vector3.down, out hit, Mathf.Infinity, whatIsLand.AddToMask(whatIsWater)))
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

		public virtual void HandleGravity ()
		{
			if (controller != null && controller.enabled && !controller.isGrounded)
			{
				yVel -= gravity * Time.deltaTime;
				move += Vector3.up * yVel;
			}
			else
				yVel = 0;
		}

		public virtual void OnDisable ()
		{
			GameManager.updatables = GameManager.updatables.Remove(this);
		}
	}
}