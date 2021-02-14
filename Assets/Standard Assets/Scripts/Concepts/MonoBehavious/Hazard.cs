using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class Hazard : Spawnable
	{
		public float damage;
		public bool destroyOnContact;
		[HideInInspector]
		public bool dead;
		
		public virtual void OnCollisionEnter (Collision coll)
		{
			if (dead)
				return;
			IDestructable destructable = coll.gameObject.GetComponent<IDestructable>();
			if (destructable != null)
				ApplyDamage (destructable, damage);
			if (destroyOnContact)
			{
				dead = true;
				if (prefabIndex == -1)
					Destroy(gameObject);
				else
					// ObjectPool.Instance.Despawn (prefabIndex, gameObject, trs);
					Destroy(gameObject);
			}
		}
		
		public virtual void ApplyDamage (IDestructable destructable, float amount)
		{
			destructable.TakeDamage (amount, this);
		}
	}
}