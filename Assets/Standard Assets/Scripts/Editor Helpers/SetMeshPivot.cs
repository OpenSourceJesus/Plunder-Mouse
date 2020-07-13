#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetMeshPivot : MonoBehaviour
{
	public bool update;
	public Transform trs;
	public MeshFilter[] meshFilters;
	
	public virtual void Update ()
	{
		if (!update)
			return;
		update = false;
		foreach (MeshFilter meshFilter in meshFilters)
			meshFilter.GetComponent<Transform>().position += meshFilter.GetComponent<Transform>().rotation * (trs.position - meshFilter.sharedMesh.bounds.center);
	}
}
#endif