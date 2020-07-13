using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using FastMemberCore;
using System.Reflection;

namespace PlunderMouse
{
	public class Enemy : MoveableEntity, IDestructable, ISpawnable, IUpdatable
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
		public float followRange;
		public float interestRange;
		[HideInInspector]
		public float interestRangeSqr;
		[HideInInspector]
		public bool awakened;
		[HideInInspector]
		public float hp;
		public float Hp
		{
			get
			{
				return hp;
			}
			set
			{
				hp = value;
			}
		}
		public int maxHp;
		public int MaxHp
		{
			get
			{
				return maxHp;
			}
			set
			{
				maxHp = value;
			}
		}
		public _Animator anim;
		public _Animator2 anim2;
		[HideInInspector]
		public bool dead;
		public Collider[] colliders;
		public LookAtActivePlayerObject lookAtPlayer;
		public Patrol patrolNonflying;
		[HideInInspector]
		public bool shoot;
		public AutoClickButton triggerOnDeath;
		public string idleAnimName;
		public string hurtAnimName;
		public string attackAnimName;
		public string dieAnimName;
		public int prefabIndex;
		public int PrefabIndex
		{
			get
			{
				return prefabIndex;
			}
		}
		public int despawnDelay;
		[HideInInspector]
		public Vector3 toPlayer;
		public static List<Enemy> enemies = new List<Enemy>();
		public LayerMask whatBlocksVision;
		public SphereCollider visionRangeSphereCollider;
		public SphereCollider awakenRangeSphereCollider;
		float awakenRangeSqr;
		public Transform eyeTrs;
		public float visionDegrees;
		public bool takeCollisionDamage;
		public float minCollisionIntensityForDamage;
		public float damagePerCollisionIntensity;
		public EnemyGroup enemyGroup;
		public bool disableCollidersOnDeath;
		public MakeMagicIndicator makeMagicIndicator;
		
		public virtual void Awake ()
		{
			foreach (Collider collider in colliders)
			{
				foreach (Collider collider2 in colliders)
				{
					if (collider != collider2)
						Physics.IgnoreCollision(collider, collider2, true);
				}
			}
			anim.Init ();
			awakenRangeSqr = awakenRangeSphereCollider.bounds.extents.x;
			awakenRangeSqr *= awakenRangeSqr;
			interestRangeSqr = interestRange * interestRange;
			if (anim2 != null)
				anim2.Play ("Attack");
		}
		
		public virtual void OnEnable ()
		{
			Hp = MaxHp;
			dead = false;
			foreach (Collider collider in colliders)
				collider.enabled = true;
			anim.Stop (true);
			anim.Play (idleAnimName);
			awakenRangeSphereCollider.enabled = true;
			visionRangeSphereCollider.enabled = true;
			if (patrolNonflying != null)
				patrolNonflying.enabled = true;
			makeMagicIndicator.enabled = true;
		}
		
		public virtual void DoUpdate ()
		{
			// if (!awakened || dead)
			// 	return;
			move = Vector3.zero;
			toPlayer = PlayerObject.CurrentActive.trs.position - trs.position;
			if (toPlayer.sqrMagnitude > interestRangeSqr)
				LoseInterest ();
			HandleMovement ();
			HandleGravity ();
			if (controller != null && controller.enabled)
				controller.Move(move * Time.deltaTime);
			else
				rigid.velocity = move * multiplyRigidMoveSpeed;
		}
		
		public virtual void HandleMovement ()
		{
			if (Vector3.Distance(trs.position, PlayerObject.CurrentActive.trs.position) >= followRange)
				Move ();
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
		
		public virtual void Move ()
		{
			move = toPlayer.normalized * moveSpeed;
		}
		
		public virtual void TakeDamage (float amount, Hazard source)
		{
			if (dead)
				return;
			float previousHp = hp;
			hp = Mathf.Clamp(hp - amount, 0, MaxHp);
			if (GameManager.GetSingleton<Survival>() != null)
				GameManager.GetSingleton<Survival>().AddScore (previousHp - hp);
			anim.Stop (true);
			if (!string.IsNullOrEmpty(hurtAnimName))
				anim.Play (hurtAnimName);
			if (hp == 0)
			{
				dead = true;
				Death ();
			}
			else if (!awakened)
				Awaken ();
		}

		public virtual void OnDisable ()
		{
			StopAllCoroutines();
			GameManager.updatables = GameManager.updatables.Remove(this);
		}
		
		public virtual void OnTriggerStay (Collider other)
		{
			RaycastHit hit = new RaycastHit();
			if (!awakened && ((Vector3.Angle(eyeTrs.forward, PlayerObject.CurrentActive.trs.position - eyeTrs.position) <= visionDegrees && Physics.Raycast(eyeTrs.position, PlayerObject.CurrentActive.trs.position - eyeTrs.position, out hit, visionRangeSphereCollider.bounds.extents.x, whatBlocksVision) && hit.rigidbody == PlayerObject.CurrentActive.rigid) || (PlayerObject.CurrentActive.trs.position - trs.position).sqrMagnitude <= awakenRangeSqr))
			{
				if (enemyGroup != null)
				{
					enemyGroup.enabled = false;
					foreach (Enemy enemy in enemyGroup.enemies)
					{
						if (!enemy.awakened)
							enemy.Awaken ();
					}
				}
				else
					Awaken ();
			}
		}

		public virtual void Awaken ()
		{
			awakened = true;
			if (lookAtPlayer != null)
				lookAtPlayer.enabled = true;
			if (patrolNonflying != null)
				patrolNonflying.enabled = false;
			awakenRangeSphereCollider.enabled = false;
			visionRangeSphereCollider.enabled = false;
			GameManager.updatables = GameManager.updatables.Add(this);
		}

		public virtual void LoseInterest ()
		{
			awakened = false;
			if (lookAtPlayer != null)
				lookAtPlayer.enabled = false;
			if (enemyGroup != null)
			{
				foreach (Enemy enemy in enemyGroup.enemies)
				{
					if (enemy.awakened)
						LoseInterest ();
				}
				enemyGroup.enabled = true;
			}
			else if (patrolNonflying != null)
				patrolNonflying.enabled = true;
			awakenRangeSphereCollider.enabled = true;
			visionRangeSphereCollider.enabled = true;
			GameManager.updatables = GameManager.updatables.Remove(this);
		}
		
		public virtual void Death ()
		{
			anim.Stop (true);
			anim.Play (dieAnimName);
			if (disableCollidersOnDeath)
			{
				foreach (Collider collider in colliders)
					collider.enabled = false;
			}
			if (lookAtPlayer != null)
				lookAtPlayer.enabled = false;
			if (triggerOnDeath != null)
				triggerOnDeath.Trigger ();
			SoundEffect.Settings deathSoundSettings = new SoundEffect.Settings();
			deathSoundSettings.audioClip = GameManager.GetSingleton<AudioManager>().deathSounds[Random.Range(0, GameManager.GetSingleton<AudioManager>().deathSounds.Length)];
			deathSoundSettings.speakerTrs = trs;
			GameManager.GetSingleton<AudioManager>().MakeSoundEffect (deathSoundSettings);
			makeMagicIndicator.enabled = false;
			if (prefabIndex != -1)
				GameManager.GetSingleton<ObjectPool>().DelayDespawn (prefabIndex, gameObject, trs, despawnDelay);
			else
				Destroy(gameObject, despawnDelay);
			GameManager.updatables = GameManager.updatables.Remove(this);
		}

		public virtual void OnCollisionEnter (Collision coll)
		{
			if (!takeCollisionDamage)
				return;
			float collisionIntensity = coll.impulse.magnitude * Time.deltaTime;
			if (collisionIntensity >= minCollisionIntensityForDamage)
				TakeDamage (collisionIntensity * damagePerCollisionIntensity, null);
		}
	}
}