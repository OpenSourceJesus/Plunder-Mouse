using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace Extensions
{
	public static class QuaternionExtensions
	{
		public static Quaternion NULL = new Quaternion(MathfExtensions.NULL_FLOAT, MathfExtensions.NULL_FLOAT, MathfExtensions.NULL_FLOAT, MathfExtensions.NULL_FLOAT);

		public static Quaternion ToQuat (this QuaternionControl control)
		{
			return new Quaternion(control.x.ReadValue(), control.y.ReadValue(), control.z.ReadValue(), control.w.ReadValue());
		}
	}
}