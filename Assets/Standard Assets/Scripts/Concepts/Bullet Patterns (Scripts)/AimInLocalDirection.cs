using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class AimInLocalDirection : BulletPattern
	{
		public Vector3 shootDirection;

		public override Vector3 GetShootDirection (Transform spawner)
		{
			return spawner.rotation * shootDirection;
		}
	}
}