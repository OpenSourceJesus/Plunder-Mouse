#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using Extensions;

[ExecuteInEditMode]
public class SnapPosition : MonoBehaviour
{
	public bool useLocalPosition;
	public Transform trs;
	Vector3 newPos;
	public Vector3 snapTo;
	public Vector3 offset;
	
	public virtual void OnEnable ()
	{
		if (Application.isPlaying)
		{
			Destroy(this);
			return;
		}
		if (trs == null) 
			trs = GetComponent<Transform>();
	}
	
	public virtual void Update ()
	{
		if (!useLocalPosition)
		{
			newPos = trs.position.Snap(snapTo);
			if (snapTo.x == 0)
				newPos.x = trs.position.x;
			if (snapTo.y == 0)
				newPos.y = trs.position.y;
			if (snapTo.z == 0)
				newPos.z = trs.position.z;
			trs.position = newPos + offset;
		}
		else
		{
			newPos = trs.localPosition.Snap(snapTo);
			if (snapTo.x == 0)
				newPos.x = trs.localPosition.x;
			if (snapTo.y == 0)
				newPos.y = trs.localPosition.y;
			if (snapTo.z == 0)
				newPos.z = trs.localPosition.z;
			trs.localPosition = newPos + offset;
		}
	}
}
#endif