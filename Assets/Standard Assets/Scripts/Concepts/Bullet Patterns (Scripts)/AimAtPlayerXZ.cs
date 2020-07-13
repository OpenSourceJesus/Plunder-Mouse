using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimAtPlayerXZ : BulletPattern
	{
		public override Vector3 GetShootDirection (Transform spawner)
		{
			return (PlayerObject.CurrentActive.trs.position - spawner.position).GetXZ();
		}
	}
}