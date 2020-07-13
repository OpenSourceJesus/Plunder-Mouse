using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	[CreateAssetMenu]
	public class ChoseFromLocalRotationsThenShootBulletPattern : BulletPattern
	{
		public BulletPattern bulletPattern;
		public ChoseMethod choseMethod;
		public Vector3[] localRotations;
		public float accuracy = 1;
		
		public override Bullet[] Shoot (Transform spawner, Bullet bulletPrefab, float positionOffset = 0)
		{
			int indexToUse = -1;
			if (choseMethod == ChoseMethod.Random)
				indexToUse = Random.Range(0, localRotations.Length);
			else if (choseMethod == ChoseMethod.LoopForwards)
			{
				indexToUse = IndexOfCurrentRotationInArray(spawner) + 1;
				if (indexToUse == localRotations.Length)
					indexToUse = 0;
			}
			else if (choseMethod == ChoseMethod.LoopBackwards)
			{
				indexToUse = IndexOfCurrentRotationInArray(spawner) - 1;
				if (indexToUse == -1)
					indexToUse = localRotations.Length - 1;
			}
			spawner.localEulerAngles = localRotations[indexToUse];
			Bullet[] output = bulletPattern.Shoot (spawner, bulletPrefab, positionOffset);
			return output;
		}

		public virtual int IndexOfCurrentRotationInArray (Transform spawner)
		{
			for (int i = 0; i < localRotations.Length; i ++)
			{
				if (Quaternion.Angle(Quaternion.Euler(localRotations[i]), Quaternion.Euler(spawner.localEulerAngles)) <= accuracy)
					return i;
			}
			return -1;
		}

        public enum ChoseMethod
        {
            Random,
            LoopForwards,
			LoopBackwards
        }
	}
}