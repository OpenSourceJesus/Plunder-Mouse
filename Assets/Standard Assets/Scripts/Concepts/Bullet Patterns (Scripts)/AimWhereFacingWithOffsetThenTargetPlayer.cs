using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimWhereFacingWithOffsetThenTargetPlayer : AimWhereFacingThenTargetPlayer
	{
		// [MakeConfigurable]
		public float shootOffset;
		
		public override Vector3 GetShootDirection (Transform spawner)
		{
			return VectorExtensions.Rotate(base.GetShootDirection(spawner), shootOffset);
		}
	}
}