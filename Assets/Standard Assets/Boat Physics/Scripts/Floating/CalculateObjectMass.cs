using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateObjectMass : MonoBehaviour 
{
	public float massMultiplier;
	public new Renderer renderer;
	public Rigidbody rigid;

	public virtual void Start ()
	{
		SetMass ();
	}

	public virtual void SetMass ()
	{
		float volume = renderer.bounds.size.x * renderer.bounds.size.y * renderer.bounds.size.z;
		float rhoWater = 1027f;
		float densityIce = rhoWater * 0.70f;
		float mass = densityIce * volume;
		rigid.mass = mass * massMultiplier;
	}
}
