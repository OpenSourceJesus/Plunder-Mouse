using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[ExecuteInEditMode]
public class _Canvas : MonoBehaviour
{
	[HideInInspector]
	public Canvas canvas;
	public float planeDistance;
	
	public virtual void Start ()
	{
		if (!Application.isPlaying)
			canvas = GetComponent<Canvas>();
		else
		{
			canvas.worldCamera = Camera.main;
			canvas.planeDistance = planeDistance;
		}
	}
	
	public virtual void Update ()
	{
		Canvas.ForceUpdateCanvases();
	}
}
