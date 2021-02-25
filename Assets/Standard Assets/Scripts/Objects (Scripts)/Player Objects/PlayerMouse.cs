using UnityEngine;
using Extensions;
using UnityEngine.InputSystem;

namespace PlunderMouse
{
	public class PlayerMouse : PlayerObject
	{
		public new static PlayerMouse instance;
		public new static PlayerMouse Instance
		{
			get
			{
				if (instance == null)
					instance = FindObjectOfType<PlayerMouse>();
				return instance;
			}
		}
		[HideInInspector]
		public Vector3 positionOffsetFromShip;
		public CharacterController controller;
		public float jumpSpeed;
		public float jumpDuration;
		public float gravity;
		public float maxHitNormalAngleToJumpOnBulletWithImpunity;
		public float multiplyRigidMoveSpeed;
		public float groundCheckDistance;
		public LayerMask whatICollideWith;
		public Transform groundCheckPoint;
		public float slideRate;
		// public bool isSwimming;
		bool isGrounded;
		float timeLastGrounded;
		float yVel;
		Vector3 previousPosition;
		Quaternion previousRotation;
		bool canJump;
		
		public override void Start ()
		{
			base.Start();
			positionOffsetFromShip = trs.localPosition;
			if (switchIndicator != null)
				switchIndicator.SetActive(false);
			if (!Active)
				switchIndicatorTrigger.gameObject.SetActive(false);
		}
		
		public override void DoUpdate ()
		{
			attackTimer += Time.deltaTime;
			if (dead || !active)
				return;
			move = Vector3.zero;
			isGrounded = controller.isGrounded;
			if (isGrounded)
			{
				timeLastGrounded = Time.time;
				canJump = true;
			}
			HandleFacing ();
			Move ();
			HandleAttacking ();
			// HandleBoarding ();
			HandleGravity ();
			HandleJump ();
			if (controller.enabled)
				controller.Move(move * Time.deltaTime);
		}

		void HandleFacing ()
		{
			if (!weapon.anim.isPlaying)
			{
				trs.forward = OVRCameraRig.Instance.eyesTrs.forward.SetY(0);
				previousRotation = trs.rotation;
			}
			else
				trs.rotation = previousRotation;
		}
		
		void HandleAttacking ()
		{
			weapon.trs.parent.rotation = OVRCameraRig.CurrentHand.rotation;
			if (attackTimer > attackRate && !weapon.anim.isPlaying)
			{
				if (InputManager.RightAttackInput)
				{
					weapon.trs.parent.localScale = weapon.trs.parent.localScale.SetX(1);
					attackTimer = 0;
					Attack ();
				}
				else if (InputManager.LeftAttackInput)
				{
					weapon.trs.parent.localScale = weapon.trs.parent.localScale.SetX(-1);
					attackTimer = 0;
					Attack ();
				}
			}
		}
		
		void HandleGravity ()
		{
			if (controller.enabled && !isGrounded)
			{
				yVel -= gravity * Time.deltaTime;
				move += Vector3.up * yVel;
			}
		}
		
		// void HandleBoarding ()
		// {
		// 	canSwitch = switchIndicatorTrigger.collidersInside.Count > 0;
		// 	if (switchIndicator != null)
		// 		switchIndicator.SetActive(canSwitch);
		// 	// if (canSwitch && (OVRInput.GetDown(OVRInput.Button.Three) || InputManager.inputter.GetButtonDown("Interact")))
		// 		// BoardShip ();
		// }
		
		void Attack ()
		{
			weapon.Attack ();
		}
		
		public override void Move ()
		{
			move += GetMoveInput();
			if (controller.enabled)
				move *= moveSpeed;
			else
				rigid.velocity = (move * moveSpeed * multiplyRigidMoveSpeed).SetY(rigid.velocity.y);
		}
		
		void HandleJump ()
		{
			if (canJump && InputManager.JumpInput && Time.time - timeLastGrounded < jumpDuration)
			{
				if (isGrounded)
					yVel = 0;
				Jump ();
			}
			else
			{
				if (yVel > 0)
					yVel = 0;
				canJump = false;
			}
		}
		
		public virtual void Jump ()
		{
			if (controller.enabled)
			{
				yVel += jumpSpeed * Time.deltaTime;
				move += Vector3.up * yVel;
			}
		}

		void OnCollisionEnter (Collision coll)
		{
			Bullet hitBullet = coll.gameObject.GetComponent<Bullet>();
			if (hitBullet != null && (Vector3.Angle(coll.GetContact(0).normal, Vector3.up) > maxHitNormalAngleToJumpOnBulletWithImpunity || !InputManager.JumpInput))
			{
				TakeDamage (hitBullet.damage, hitBullet);
				Destroy(hitBullet.gameObject);
			}
			HandleSlopes ();
		}

		void OnCollisionStay (Collision coll)
		{
			HandleSlopes ();
		}

		void HandleSlopes ()
		{
			RaycastHit hit;
			if (Physics.Raycast(groundCheckPoint.position, Vector3.down, out hit, groundCheckDistance, whatICollideWith))
			{
				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
				if (slopeAngle <= controller.slopeLimit)
				{
					controller.enabled = true;
					rigid.useGravity = false;
					rigid.velocity = Vector2.zero;
				}
				else
				{
					if (controller.enabled)
						rigid.velocity = controller.velocity;
					controller.enabled = false;
					rigid.useGravity = true;
					rigid.velocity += Vector3.down * Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * slideRate * Time.deltaTime;
				}
			}
		}
		
		// void BoardShip ()
		// {
		// 	Active = false;
		// 	PlayerShip.Instance.switchIndicator.SetActive(false);
		// 	switchIndicator.SetActive(false);
		// 	switchIndicatorTrigger.gameObject.SetActive(false);
		// 	PlayerShip.Instance.Active = true;
		// 	PlayerShip.Instance.switchIndicatorTrigger.gameObject.SetActive(true);
		// 	trs.SetParent(PlayerShip.Instance.trs);
		// }
		
		// void DockShip ()
		// {
		// 	PlayerShip.Instance.Active = false;
		// 	switchIndicator.SetActive(false);
		// 	PlayerShip.Instance.switchIndicator.SetActive(false);
		// 	PlayerShip.Instance.switchIndicatorTrigger.gameObject.SetActive(false);
		// 	Active = true;
		// 	switchIndicatorTrigger.gameObject.SetActive(true);
		// 	trs.SetParent(null);
		// }
	}
}