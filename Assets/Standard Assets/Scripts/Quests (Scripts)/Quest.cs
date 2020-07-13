using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class Quest : UnlockableNode
{
	public Event[] events;
	// [HideInInspector]
	public int completionCount;
	[SaveAndLoadValue]
	public int CompletionCount
	{
		get
		{
			return completionCount;
		}
		set
		{
			completionCount = value;
			if (value >= minCompletionCount && value <= maxCompletionCount)
			{
				// GameManager.GetSingleton<ObjectiveGuider>().gameObject.SetActive(false);
				Traverse ();
				QuestManager.currentQuests.Remove(this);
			}
		}
	}
	public int minCompletionCount = 1;
	public int maxCompletionCount = 1;
	public string[] locations;
#if UNITY_EDITOR
	[HideInInspector]
	public Quest questPrefab;
	string assetPath;
	public bool deleteQuest;
	
	public void Refresh ()
	{
		if (questPrefab != null)
		{
			assetPath = QuestData.instance.questsFolderPath + "/" + questPrefab.gameObject.name + ".prefab";
			
			AssetDatabase.DeleteAsset(assetPath);
			if (deleteQuest)
			{
				DestroyImmediate(gameObject);
				return;
			}
		}
		assetPath = QuestData.instance.questsFolderPath + "/" + name + ".prefab";
		if (questPrefab == null || questPrefab.name != name)
			questPrefab = PrefabUtility.CreatePrefab(assetPath, gameObject).GetComponent<Quest>();
	}
#endif
	
	public override void Unlock ()
	{
		base.Unlock ();
		foreach (Event _event in events)
		{
			if (_event.type == EventType.OnUnlock)
				_event.Trigger ();
		}
	}
	
	public override void Traverse ()
	{
		base.Traverse ();
		foreach (Event _event in events)
		{
			if (_event.type == EventType.OnComplete)
				_event.Trigger ();
		}
	}
	
	[Serializable]
	public class Event
	{
		public EventType type;
		public ActivateEntry[] actions;
		
		public void Trigger ()
		{
			foreach (ActivateEntry action in actions)
			{
				GameObject go;
				switch (action.activate)
				{
					case ActivateState.Activate:
						if (QuestManager.activatableGos.TryGetValue(action.goName, out go))
							go.SetActive(true);
						else
							GameObject.Find(action.goName).SetActive(true);
						break;
					case ActivateState.Deactivate:
						if (QuestManager.deactivatableGos.TryGetValue(action.goName, out go))
							go.SetActive(false);
						else
							GameObject.Find(action.goName).SetActive(false);
						break;
					case ActivateState.Toggle:
						if (QuestManager.activatableGos.TryGetValue(action.goName, out go))
							go.SetActive(true);
						else if (QuestManager.deactivatableGos.TryGetValue(action.goName, out go))
							go.SetActive(false);
						else
						{
							go = GameObject.Find(action.goName);
							go.SetActive(!go.activeSelf);
						}
						break;
				}
			}
		}
	}
	
	[Serializable]
	public class ActivateEntry
	{
		public string goName;
		public ActivateState activate;
	}
	
	public enum ActivateState
	{
		Activate = 0,
		Deactivate = 1,
		Toggle = 2,
	}
	
	public enum EventType
	{
		OnUnlock = 0,
		OnStart = 1,
		OnComplete = 2,
	}
}
