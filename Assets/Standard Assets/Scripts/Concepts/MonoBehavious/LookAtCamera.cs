using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using GameDevJourney;

public class LookAtCamera : UpdateWhileEnabled, IUpdatable
{
	public Transform trs;
	
	public virtual void DoUpdate ()
	{
		trs.forward = OVRCameraRig.Instance.eyesTrs.position - trs.position;
	}
}
