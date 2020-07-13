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
			rangedDespawn = GameManager.GetSingleton<ObjectPool>().RangeDespawn (prefabIndex, gameObject, trs, range);
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
				deathResponseSettings.audioClip = GameManager.GetSingleton<AudioManager>().deathResponses[Random.Range(0, GameManager.GetSingleton<AudioManager>().deathResponses.Length)];
				deathResponseSettings.persistant = true;
				SoundEffect soundEffect = GameManager.GetSingleton<AudioManager>().MakeSoundEffect (deathResponseSettings);
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
			GameManager.GetSingleton<ObjectPool>().CancelRangedDespawn (rangedDespawn);
		}
	}
}