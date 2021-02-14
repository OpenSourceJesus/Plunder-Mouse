using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGuider : SingletonMonoBehaviour<ObjectiveGuider>
{
	public Transform trs;
	public Transform location;
	
	public virtual void Start ()
	{
		gameObject.SetActive(false);
	}
	
	public virtual void DoUpdate ()
	{
		trs.forward = location.position - trs.position;
	}
}
