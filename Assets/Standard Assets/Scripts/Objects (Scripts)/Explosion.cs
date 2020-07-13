using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class Explosion : Hazard
	{
		public virtual void OnTriggerEnter (Collider other)
		{
			if (dead)
				return;
			dead = true;
			IDestructable destructable = other.GetComponent<IDestructable>();
			if (destructable != null)
				ApplyDamage (destructable, damage);
		}

		public virtual void DestroyMe ()
		{
			Destroy(gameObject);
		}
	}
}