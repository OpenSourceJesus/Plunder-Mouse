using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimAtPlayerXZWithOffset : AimAtPlayerXZ
	{
		// [MakeConfigurable]
		public Vector3 shootOffset;
		
		public override Vector3 GetShootDirection (Transform spawner)
		{
			return base.GetShootDirection(spawner).Rotate(Quaternion.Euler(shootOffset));
		}
	}
}