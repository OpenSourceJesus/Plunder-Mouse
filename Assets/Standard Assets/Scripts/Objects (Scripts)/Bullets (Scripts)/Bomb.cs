using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class Bomb : Bullet
	{
		bool exploded;
		public bool explodeOnContact;
		public Explosion explosionPrefab;
		public Timer explodeDelayTimer;

		public virtual void Awake ()
		{
			explodeDelayTimer.onFinished += Explode;
		}

		public override void OnDestroy ()
		{
			explodeDelayTimer.onFinished -= Explode;
		}

		public override void OnCollisionEnter (Collision coll)
		{
			if (!exploded && explodeOnContact)
			{
				exploded = true;
				explodeDelayTimer.Reset ();
				explodeDelayTimer.Start ();
			}
			base.OnCollisionEnter (coll);
		}

		public virtual void Explode (params object[] args)
		{
			exploded = true;
			GameManager.GetSingleton<ObjectPool>().SpawnComponent<Explosion>(explosionPrefab, trs.position);
			// GameManager.GetSingleton<ObjectPool>().Despawn (prefabIndex, gameObject, trs);
			Destroy(gameObject);
		}

		public override void OnEnable ()
		{
			rigid.velocity = trs.forward * moveSpeed;
		}

		public override void OnDisable ()
		{
			base.OnDisable ();
			collider.enabled = false;
			exploded = false;
		}
	}
}