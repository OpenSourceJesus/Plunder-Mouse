using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class SlimeEnemy : ShootingEnemy
	{
		public Transform spawnsParent;
		
		public override void DoUpdate ()
		{
			trs.forward = (PlayerObject.CurrentActive.trs.position - trs.position).SetY(0);
			spawnsParent.forward = PlayerObject.CurrentActive.trs.position - spawnsParent.position;
			base.DoUpdate ();
		}
	}
}