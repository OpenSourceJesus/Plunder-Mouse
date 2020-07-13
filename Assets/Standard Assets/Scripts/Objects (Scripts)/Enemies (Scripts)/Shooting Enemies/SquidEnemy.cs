using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class SquidEnemy : ShootingEnemy
	{
		public float dartSpeed;
		public float dartStopDelay;
		public float maxDartDist;
		float maxDartDistSqr;
		bool isDarting;
		public CapsuleCollider capsuleCollider;
		float distToCapsuleSphere;
		public float attackRange;
		float attackRangeSqr;
		public ColliderPresenceDetector playerDetector;
		public float damage;

		public override void Awake ()
		{
			base.Awake ();
			distToCapsuleSphere = capsuleCollider.height - capsuleCollider.radius * 2; 
			maxDartDistSqr = maxDartDist * maxDartDist;
			attackRangeSqr = attackRange * attackRange;
		}

		public override void DoUpdate ()
		{
			move = Vector3.zero;
			toPlayer = PlayerObject.CurrentActive.trs.position - trs.position;
			if (toPlayer.sqrMagnitude > interestRangeSqr)
				LoseInterest ();
			HandleRotation ();
			HandleDarting ();
			HandleMovement ();
			HandleGravity ();
			HandleAttacking ();
			if (controller != null && controller.enabled)
				controller.Move(move * Time.deltaTime);
			else
				rigid.velocity = move * multiplyRigidMoveSpeed;
		}

		public virtual void HandleDarting ()
		{
			if (!isDarting && toPlayer.SetY(0).sqrMagnitude <= maxDartDistSqr)
			{
				isDarting = true;
				StartCoroutine(DartRoutine ());
			}
		}

		public override void HandleAttacking ()
		{
			if (!anim.GetCurrentlyPlayingAnimationNames().Contains(attackAnimName))
			{
				anim.Stop (true);
				if (toPlayer.sqrMagnitude <= attackRangeSqr)
				{
					anim.Play (attackAnimName);
					playerDetector.enabled = true;
				}
			}
			else if (playerDetector.collidersInside.Count > 0)
			{
				PlayerObject.CurrentActive.TakeDamage (damage, null);
				playerDetector.enabled = false;
			}
		}

		public override void HandleMovement ()
		{
			Move (trs.forward);
		}

		public virtual void HandleRotation ()
		{
			if (!isDarting)
				trs.forward = toPlayer.SetY(0);
		}

		public virtual IEnumerator DartRoutine ()
		{
			bool previousShouldStop = false;
			float timeSinceShouldStop = Mathf.Infinity;
			do
			{
				yield return new WaitForEndOfFrame();
				if (!Physics.CapsuleCast(trs.position - (trs.rotation * Vector3.right * distToCapsuleSphere), trs.position + (trs.rotation * Vector3.right * distToCapsuleSphere), capsuleCollider.radius, trs.forward, maxDartDist, LayerMask.GetMask(LayerMask.LayerToName(PlayerObject.CurrentActive.gameObject.layer))))
				{
					if (!previousShouldStop)
						timeSinceShouldStop = Time.time;
					else if (Time.time - timeSinceShouldStop > dartStopDelay)
						break;
					previousShouldStop = true;
				}
				else
					previousShouldStop = false;
			} while (true);
			isDarting = false;
		}

		public virtual void Move (Vector3 move)
		{
			move = Vector3.ClampMagnitude(move, 1);
			if (isDarting)
				this.move = (move * dartSpeed).SetY(rigid.velocity.y);
			else
				this.move = (move * moveSpeed).SetY(rigid.velocity.y);
			if (controller != null)
				this.move = this.move.SetY(0);
		}
	}
}