using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class GrubEnemy : ShootingEnemy
	{
		public Transform spawnsParent;

		public override void DoUpdate ()
		{
			trs.forward = PlayerObject.CurrentActive.trs.position - trs.position;
			spawnsParent.forward = PlayerObject.CurrentActive.trs.position - spawnsParent.position;
			base.DoUpdate ();
		}
	}
}