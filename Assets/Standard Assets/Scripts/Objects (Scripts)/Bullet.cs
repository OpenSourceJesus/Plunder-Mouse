using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class Bullet : Hazard
	{
		public float range;
		public Rigidbody rigid;
		public float moveSpeed;
		public Transform shooter;
		public ObjectPool.RangedDespawn rangedDespawn;
		public new Collider collider;
		
		public virtual void OnEnable ()
		{
			dead = false;
			rangedDespawn = ObjectPool.Instance.RangeDespawn (prefabIndex, gameObject, trs, range);
			rigid.velocity = trs.forward * moveSpeed;
		}
		
		public override void ApplyDamage (IDestructable destructable, float amount)
		{
			if (destructable.Hp == 0)
				return;
			base.ApplyDamage (destructable, amount);
			if (destructable.Hp == 0 && shooter != null)
			{
				SoundEffect.Settings deathResponseSettings = new SoundEffect.Settings();
				deathResponseSettings.audioClip = AudioManager.Instance.deathResponses[Random.Range(0, AudioManager.Instance.deathResponses.Length)];
				deathResponseSettings.persistant = true;
				SoundEffect soundEffect = AudioManager.Instance.MakeSoundEffect (deathResponseSettings);
				soundEffect.trs.SetParent(shooter);
				soundEffect.trs.localPosition = Vector3.zero;
				soundEffect.trs.localEulerAngles = Vector3.zero;
			}
		}

		public virtual void OnDisable ()
		{
			StopAllCoroutines();
		}

		public virtual void OnDestroy ()
		{
			ObjectPool.Instance.CancelRangedDespawn (rangedDespawn);
		}
	}
}