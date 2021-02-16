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
		float timeLastGrounded;
		float yVel;
		Vector3 previousPosition;
		// public bool isSwimming;
		Quaternion previousRotation;
		bool canJump;
		public LayerMask whatICollideWith;
		
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
			if (controller.isGrounded)
			{
				timeLastGrounded = Time.time;
				canJump = true;
			}
			HandleFacing ();
			Move ();
			HandleAttacking ();
			HandleBoarding ();
			HandleGravity ();
			HandleJump ();
			controller.Move(move * Time.deltaTime);
		}

		public virtual void HandleFacing ()
		{
			if (!weapon.anim.isPlaying)
			{
				trs.forward = OVRCameraRig.Instance.eyesTrs.forward.SetY(0);
				previousRotation = trs.rotation;
			}
			else
				trs.rotation = previousRotation;
		}
		
		public virtual void HandleAttacking ()
		{
			weapon.trs.parent.rotation = OVRCameraRig.CurrentHand.rotation;
			if (attackTimer > attackRate && !weapon.anim.isPlaying)
			{
				if ((InputManager.Instance.inputDevice == InputManager.InputDevice.KeyboardAndMouse && Mouse.current.leftButton.isPressed) || (InputManager.Instance.inputDevice == InputManager.InputDevice.OculusRift && InputManager.LeftTouchController.trigger.ReadValue() >= InputManager.Settings.defaultDeadzoneMin))
				{
					weapon.trs.parent.localScale = weapon.trs.parent.localScale.SetX(1);
					attackTimer = 0;
					Attack ();
				}
				else if ((InputManager.Instance.inputDevice == InputManager.InputDevice.KeyboardAndMouse && Mouse.current.rightButton.isPressed) || (InputManager.Instance.inputDevice == InputManager.InputDevice.OculusRift && InputManager.RightTouchController.trigger.ReadValue() >= InputManager.Settings.defaultDeadzoneMin))
				{
					weapon.trs.parent.localScale = weapon.trs.parent.localScale.SetX(-1);
					attackTimer = 0;
					Attack ();
				}
			}
		}
		
		public virtual void HandleGravity ()
		{
			if (controller.enabled && !controller.isGrounded)
			{
				yVel -= gravity * Time.deltaTime;
				move += Vector3.up * yVel;
			}
		}
		
		public virtual void HandleBoarding ()
		{
			canSwitch = switchIndicatorTrigger.collidersInside.Count > 0;
			if (switchIndicator != null)
				switchIndicator.SetActive(canSwitch);
			// if (canSwitch && (OVRInput.GetDown(OVRInput.Button.Three) || InputManager.inputter.GetButtonDown("Interact")))
				// BoardShip ();
		}
		
		public virtual void Attack ()
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
		
		public virtual void HandleJump ()
		{
			if (canJump && InputManager.JumpInput && Time.time - timeLastGrounded < jumpDuration)
			{
				if (controller.isGrounded)
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

		public virtual void OnCollisionEnter (Collision coll)
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
		
		public virtual void BoardShip ()
		{
			Active = false;
			PlayerShip.Instance.switchIndicator.SetActive(false);
			switchIndicator.SetActive(false);
			switchIndicatorTrigger.gameObject.SetActive(false);
			PlayerShip.Instance.Active = true;
			PlayerShip.Instance.switchIndicatorTrigger.gameObject.SetActive(true);
			trs.SetParent(PlayerShip.Instance.trs);
		}
		
		public virtual void DockShip ()
		{
			PlayerShip.Instance.Active = false;
			switchIndicator.SetActive(false);
			PlayerShip.Instance.switchIndicator.SetActive(false);
			PlayerShip.Instance.switchIndicatorTrigger.gameObject.SetActive(false);
			Active = true;
			switchIndicatorTrigger.gameObject.SetActive(true);
			trs.SetParent(null);
		}

		void HandleSlopes ()
		{
			RaycastHit hit;
			if (Physics.Raycast(controller.bounds.center + Vector3.down * controller.bounds.extents.y, Vector3.down, out hit, groundCheckDistance, whatICollideWith))
			{
				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
				if (slopeAngle <= controller.slopeLimit)
				{
					controller.enabled = true;
					rigid.useGravity = false;
					rigid.velocity = Vector2.zero;
					return;
				}
				// else
				// 	return;
			}
			else
				return;
			if (controller.enabled)
				rigid.velocity = controller.velocity;
			controller.enabled = false;
			rigid.useGravity = true;
		}
	}
}