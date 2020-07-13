using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class LookAtCamera : MonoBehaviour, IUpdatable
{
	public Transform trs;

	public virtual void OnEnable ()
	{
		GameManager.updatables = GameManager.updatables.Add(this);
	}
	
	public virtual void DoUpdate ()
	{
		trs.forward = GameManager.GetSingleton<OVRCameraRig>().eyesTrs.position - trs.position;
	}

	public virtual void OnDisable ()
	{
		GameManager.updatables = GameManager.updatables.Remove(this);
	}
}
