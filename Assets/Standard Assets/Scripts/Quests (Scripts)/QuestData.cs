using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{
	public static QuestData instance;
	public string questsFolderPath;
	public List<Quest> allQuests = new List<Quest>();
}
