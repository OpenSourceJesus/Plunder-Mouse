using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimAtPlayer : BulletPattern
	{
		public override Vector3 GetShootDirection (Transform spawner)
		{
			return PlayerObject.CurrentActive.trs.position - spawner.position;
		}
	}
}