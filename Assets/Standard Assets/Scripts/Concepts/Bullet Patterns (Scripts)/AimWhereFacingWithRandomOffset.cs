using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimWhereFacingWithRandomOffset : AimWhereFacing
	{
		public float randomShootOffsetRange;
		
		public override Vector3 GetShootDirection (Transform spawner)
		{
			return VectorExtensions.Rotate(base.GetShootDirection(spawner), Random.Range(-randomShootOffsetRange / 2, randomShootOffsetRange / 2));
		}
	}
}