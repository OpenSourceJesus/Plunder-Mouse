using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class MagicIndicator : Spawnable
	{
		public virtual void SetOrientation (Transform trs)
		{
			Quaternion cameraRotation = Quaternion.Euler(Vector3.up * OVRCameraRig.Instance.eyesTrs.eulerAngles.y);
			this.trs.localPosition = Quaternion.Inverse(cameraRotation) * ((trs.position - PlayerObject.CurrentActive.trs.position) / MagicLocater.Instance.range / 2);
		}
	}
}