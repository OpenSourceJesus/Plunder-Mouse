using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class Weapon : MonoBehaviour
	{
		public Transform trs;
		public Animation anim;
		public AnimationClip attackAnim;
		public float damage;
		
		public virtual void Attack ()
		{
            if (attackAnim != null)
			    anim.Play(attackAnim.name);
		}
	}
}