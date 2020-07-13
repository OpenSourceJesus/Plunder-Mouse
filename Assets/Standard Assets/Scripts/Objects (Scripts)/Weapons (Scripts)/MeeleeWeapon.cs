using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class MeeleeWeapon : Weapon, IUpdatable
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
		public new Collider collider;
		
		public override void Attack ()
		{
			base.Attack ();
			collider.enabled = true;
			GameManager.updatables = GameManager.updatables.Add(this);
		}

		public virtual void DoUpdate ()
		{
			if (!anim.isPlaying)
			{
				collider.enabled = false;
				trs.localEulerAngles = Vector3.zero;
				GameManager.updatables = GameManager.updatables.Remove(this);
			}
		}

		public virtual void OnTriggerEnter (Collider other)
		{
			collider.enabled = false;
			IDestructable destructable = other.GetComponentInParent<IDestructable>();
			if (destructable != null)
				ApplyDamage (destructable, damage);
		}
		
		public virtual void ApplyDamage (IDestructable destructable, float damage)
		{
			destructable.Hp -= damage;
		}

		public virtual void OnDestroy ()
		{
			GameManager.updatables = GameManager.updatables.Remove(this);
		}
	}
}