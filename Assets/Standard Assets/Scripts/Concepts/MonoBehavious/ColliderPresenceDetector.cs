using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ColliderPresenceDetector : MonoBehaviour
{
	[HideInInspector]
	public List<Collider> collidersInside = new List<Collider>();
	
	public virtual void OnTriggerEnter (Collider other)
	{
		if (enabled)
			collidersInside.Add(other);
	}
	
	public virtual void OnTriggerExit (Collider other)
	{
		if (enabled)
			collidersInside.Remove(other);
	}
	
	public virtual void OnDisable ()
	{
		collidersInside.Clear();
	}
}
