using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QuestManager : SingletonMonoBehaviour<QuestManager>, ISavableAndLoadable
{
#if UNITY_EDITOR
	public bool update;
#endif
	public static List<Quest> currentQuests = new List<Quest>();
	public static Dictionary<string, GameObject> activatableGos = new Dictionary<string, GameObject>();
	public static Dictionary<string, GameObject> deactivatableGos = new Dictionary<string, GameObject>();
	public QuestData questData;
	
	public virtual void OnEnable ()
	{
		if (questData != null)
			QuestData.instance = questData;
	}
	
#if UNITY_EDITOR
	public virtual void Update ()
	{
		if (update)
		{
			update = false;
			QuestData.instance.allQuests.Clear();
			foreach (Quest quest in FindObjectsOfType<Quest>())
			{
				quest.Refresh ();
				QuestData.instance.allQuests.Add(quest.questPrefab);
				for (int i = 0; i < quest.questPrefab.connections.Length; i ++)
				{
					quest.questPrefab.connections[i].end = (quest.connections[i].end as Quest).questPrefab;
				}
			}
		}
	}
#endif
	
	public static bool QuestExists (string questName)
	{
		return GetQuest(questName) != null;
	}
	
	public static Quest GetQuest (string questName)
	{
		foreach (Quest quest in QuestData.instance.allQuests)
		{
			if (quest.name == questName)
				return quest;
		}
		return null;
	}
	
	public void StartQuest (string questName)
	{
		StartQuest (GetQuest(questName));
	}
	
	public void StartQuest (Quest quest)
	{
		currentQuests.Add(quest);
		foreach (Quest.Event _event in quest.events)
		{
			if (_event.type == Quest.EventType.OnStart)
				_event.Trigger ();
		}
		GameObject location = GameObject.Find(quest.locations[0]);
		if (location != null)
		{
			ObjectiveGuider.Instance.location = location.transform;
			ObjectiveGuider.Instance.gameObject.SetActive(true);
		}
	}
	
	public void CompleteQuest (string questName)
	{
		CompleteQuest (GetQuest(questName));
	}
	
	public void CompleteQuest (Quest quest)
	{
		quest.CompletionCount ++;
	}
}
