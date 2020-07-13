using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class _Connection : MonoBehaviour
{
	public _Node start;
	public _Node end;
	public int weight;
	public Transform trs;
	public LineRenderer line;
	public Text text;
	
	public void Update ()
	{
		trs.SetParent(start.trs);
		line.sortingLayerName = "Back";
		if (end != null)
		{
			trs.position = (start.trs.position + end.trs.position) / 2;
			line.SetPositions(new Vector3[] { start.trs.position, end.trs.position });
		}
		text.text = "" + weight;
	}
}
