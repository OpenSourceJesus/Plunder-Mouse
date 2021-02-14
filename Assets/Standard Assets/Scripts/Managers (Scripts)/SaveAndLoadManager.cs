using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Extensions;
using FullSerializer;
using System;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class SaveAndLoadManager : SingletonMonoBehaviour<SaveAndLoadManager>
{
	public static fsSerializer serializer = new fsSerializer();
	// [HideInInspector]
	public List<SaveAndLoadObject> saveAndLoadObjects = new List<SaveAndLoadObject>();
	public static SaveEntry[] saveEntries;
	public static int MostRecentlyLoadedSaveEntryIndex
	{
		get
		{
			return PlayerPrefs.GetInt("Most recently loaded save entry index", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Most recently loaded save entry index", value);
		}
	}
	public static int LastSaveEntryIndex
	{
		get
		{
			return PlayerPrefs.GetInt("Last save entry index", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Last save entry index", value);
		}
	}
	// public static Dictionary<string, SaveAndLoadObject> saveAndLoadObjectTypeDict = new Dictionary<string, SaveAndLoadObject>();
	public TemporaryDisplayText displayOnSave;
	
#if UNITY_EDITOR
	public virtual void OnEnable ()
	{
		if (Application.isPlaying)
		{
			displayOnSave.obj.SetActive(false);
			return;
		}
		// saveAndLoadObjects.Clear();
		// saveAndLoadObjects.AddRange(FindObjectsOfType<SaveAndLoadObject>());
		foreach (SaveAndLoadObject saveAndLoadObject in saveAndLoadObjects)
		{
			if (saveAndLoadObject.uniqueId == MathfExtensions.NULL_INT)
				saveAndLoadObject.uniqueId = Random.Range(int.MinValue, int.MaxValue);
		}
	}
#endif
	
	public virtual void Start ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		// saveAndLoadObjectTypeDict.Clear();
		SaveAndLoadObject saveAndLoadObject;
		List<SaveEntry> saveEntries = new List<SaveEntry>();
		for (int i = 0; i < saveAndLoadObjects.Count; i ++)
		{
			saveAndLoadObject = saveAndLoadObjects[i];
			saveAndLoadObject.Init ();
			saveEntries.AddRange(saveAndLoadObject.saveEntries);
		}
		SaveAndLoadManager.saveEntries = saveEntries.ToArray();
		if (MostRecentlyLoadedSaveEntryIndex != 0)
			LoadMostRecent ();
	}
	
	public virtual void Save ()
	{
		if (SaveAndLoadManager.Instance != this)
		{
			SaveAndLoadManager.Instance.Save ();
			return;
		}
		MostRecentlyLoadedSaveEntryIndex ++;
		if (MostRecentlyLoadedSaveEntryIndex > LastSaveEntryIndex)
			LastSaveEntryIndex ++;
		for (int i = 0; i < saveEntries.Length; i ++)
			saveEntries[i].Save ();
		StartCoroutine(displayOnSave.DisplayRoutine ());
	}
	
	public virtual void Load (int savedGameIndex)
	{
		if (SaveAndLoadManager.Instance != this)
		{
			SaveAndLoadManager.Instance.Load (savedGameIndex);
			return;
		}
		MostRecentlyLoadedSaveEntryIndex = savedGameIndex;
		StartCoroutine(LoadRoutine ());
	}

	public virtual IEnumerator LoadRoutine ()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < saveEntries.Length; i ++)
			saveEntries[i].Load ();
	}
	
	public virtual void LoadMostRecent ()
	{
		Load (MostRecentlyLoadedSaveEntryIndex);
	}

	public static string Serialize (object value, Type type)
	{
		fsData data;
		serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();
		return fsJsonPrinter.CompressedJson(data);
	}
	
	public static object Deserialize (string serializedState, Type type)
	{
		fsData data = fsJsonParser.Parse(serializedState);
		object deserialized = null;
		serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();
		return deserialized;
	}
	
	public class SaveEntry
	{
		public SaveAndLoadObject saveableAndLoadObject;
		public ISavableAndLoadable saveableAndLoadable;
		public PropertyInfo[] properties;
		public FieldInfo[] fields;
		public const string VALUE_SEPERATOR = "Ⅰ";
		
		public SaveEntry ()
		{
		}
		
		public virtual void Save ()
		{
			foreach (PropertyInfo property in properties)
				PlayerPrefs.SetString(MostRecentlyLoadedSaveEntryIndex + VALUE_SEPERATOR + saveableAndLoadObject.uniqueId + VALUE_SEPERATOR + property.Name, Serialize(property.GetValue(saveableAndLoadable, null), property.PropertyType));
			foreach (FieldInfo field in fields)
				PlayerPrefs.SetString(MostRecentlyLoadedSaveEntryIndex + VALUE_SEPERATOR + saveableAndLoadObject.uniqueId + VALUE_SEPERATOR + field.Name, Serialize(field.GetValue(saveableAndLoadable), field.FieldType));
		}
		
		public virtual void Load ()
		{
			object value;
			foreach (PropertyInfo property in properties)
			{
				value = Deserialize(PlayerPrefs.GetString(MostRecentlyLoadedSaveEntryIndex + VALUE_SEPERATOR + saveableAndLoadObject.uniqueId + VALUE_SEPERATOR + property.Name, Serialize(property.GetValue(saveableAndLoadable, null), property.PropertyType)), property.PropertyType);
				property.SetValue(saveableAndLoadable, value, null);
			}
			foreach (FieldInfo field in fields)
			{
				value = Deserialize(PlayerPrefs.GetString(MostRecentlyLoadedSaveEntryIndex + VALUE_SEPERATOR + saveableAndLoadObject.uniqueId + VALUE_SEPERATOR + field.Name, Serialize(field.GetValue(saveableAndLoadable), field.FieldType)), field.FieldType);
				field.SetValue(saveableAndLoadable, value);
			}
		}
	}
}
