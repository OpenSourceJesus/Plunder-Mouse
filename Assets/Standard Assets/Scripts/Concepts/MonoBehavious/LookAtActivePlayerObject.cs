using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class LookAtActivePlayerObject : MonoBehaviour, IUpdatable
{
	public Transform trs;

	public virtual void OnEnable ()
	{
		GameManager.updatables = GameManager.updatables.Add(this);
	}
	
	public virtual void DoUpdate ()
	{
		trs.forward = (PlayerShip.CurrentActive.trs.position - trs.position).GetXZ();
	}

	public virtual void OnDisable ()
	{
		GameManager.updatables = GameManager.updatables.Remove(this);
	}
}
