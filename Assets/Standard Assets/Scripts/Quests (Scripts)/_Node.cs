using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class _Node : MonoBehaviour, ISavableAndLoadable
{
	public string title;
	public Color color;
	public Color textColor;
	public Transform trs;
	public Text text;
	public Image image;
	public _Connection[] connections;
	bool traversed;
	public bool Traversed
	{
		get
		{
			return traversed;
		}
		private set
		{
			traversed = value;
		}
	}
	
#if UNITY_EDITOR
	public _Node connectTo;
	public _Connection connectionPrefab;
	bool connectToNothingPreviously;
	public virtual void Update ()
	{
		if (Application.isPlaying)
			return;
		if (connectTo != null && connectToNothingPreviously)
		{
			_Connection connection = Instantiate(connectionPrefab);
			connection.start = this;
			connection.end = connectTo;
			connection.Update ();
			connectTo = null;
		}
		connections = GetComponentsInChildren<_Connection>();
		image.color = color;
		text.text = title;
		text.color = textColor;
		gameObject.name = title;
		connectToNothingPreviously = connectTo == null;
	}
#endif
	
	public virtual void Traverse ()
	{
		Traversed = true;
	}
}
