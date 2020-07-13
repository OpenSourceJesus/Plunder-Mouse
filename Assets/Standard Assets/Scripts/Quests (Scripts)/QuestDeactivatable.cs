using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDeactivatable : MonoBehaviour
{
	public bool startActive;
	
	public virtual void Awake ()
	{
		gameObject.SetActive(startActive);
		if (!QuestManager.deactivatableGos.ContainsKey(name))
			QuestManager.deactivatableGos.Add(name, gameObject);
	}

	public virtual void OnDestroy ()
	{
		QuestManager.deactivatableGos.Remove(name);
	}
}
