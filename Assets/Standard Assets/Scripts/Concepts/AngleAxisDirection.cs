using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;

[Serializable]
public class AngleAxisDirection
{
	public float xyAngle;
	public float xzAngle;
	public float yzAngle;
	public Vector3 direction;
	
	public Vector3 GetValue ()
	{
		Vector3 output;
		Vector2 xyVector = new Vector2();
		Vector2 xzVector = new Vector2();
		Vector2 yzVector = new Vector2();
		if (xyAngle != 0)
			xyVector = VectorExtensions.FromFacingAngle(xyAngle);
		if (xzAngle != 0)
			xzVector = VectorExtensions.FromFacingAngle(xzAngle);
		if (yzAngle != 0)
			yzVector = VectorExtensions.FromFacingAngle(yzAngle);
		output = new Vector3(xyVector.x + xzVector.x, xyVector.y + yzVector.x, xzVector.y + yzVector.y).normalized;
		direction = output;
		return output;
	}
	
	public AngleAxisDirection Add (AngleAxisDirection aad)
	{
		xyAngle += aad.xyAngle;
		xzAngle += aad.xzAngle;
		yzAngle += aad.yzAngle;
		return this;
	}
	
	public AngleAxisDirection Multiply (AngleAxisDirection aad)
	{
		xyAngle *= aad.xyAngle;
		xzAngle *= aad.xzAngle;
		yzAngle *= aad.yzAngle;
		return this;
	}
}
