using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestActivatable : MonoBehaviour
{
	public bool startActive;
	
	public virtual void Awake ()
	{
		gameObject.SetActive(startActive);
		if (!QuestManager.activatableGos.ContainsKey(name))
			QuestManager.activatableGos.Add(name, gameObject);
	}

	public virtual void OnDestroy ()
	{
		QuestManager.activatableGos.Remove(name);
	}
}
