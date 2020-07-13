using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using System;
using SaveEntry = SaveAndLoadManager.SaveEntry;
using Extensions;

public class SaveAndLoadObject : MonoBehaviour
{
	public int uniqueId = MathfExtensions.NULL_INT;
	public ISavableAndLoadable[] saveables;
	// public string typeId;
	public SaveEntry[] saveEntries;
	
	public virtual void Init ()
	{
		saveables = GetComponentsInChildren<ISavableAndLoadable>();
		// SaveAndLoadObject sameTypeObj;
		// if (!SaveAndLoadManager.saveAndLoadObjectTypeDict.TryGetValue(typeId, out sameTypeObj))
		// {
			saveEntries = new SaveEntry[saveables.Length];
			for (int i = 0; i < saveables.Length; i ++)
			{
				SaveEntry saveEntry = new SaveEntry();
				saveEntry.saveableAndLoadObject = this;
				saveEntry.saveableAndLoadable = saveables[i];
				List<PropertyInfo> saveProperties = new List<PropertyInfo>();
				saveProperties.AddRange(saveEntry.saveableAndLoadable.GetType().GetProperties());
				for (int i2 = 0; i2 < saveProperties.Count; i2 ++)
				{
					PropertyInfo property = saveProperties[i2];
					SaveAndLoadValueAttribute saveAndLoadValue = Attribute.GetCustomAttribute(property, typeof(SaveAndLoadValueAttribute)) as SaveAndLoadValueAttribute;
					if (saveAndLoadValue == null)
					{
						saveProperties.RemoveAt(i2);
						i2 --;
					}
				}
				saveEntry.properties = saveProperties.ToArray();
				
				List<FieldInfo> saveFields = new List<FieldInfo>();
				saveFields.AddRange(saveEntry.saveableAndLoadable.GetType().GetFields());
				for (int i2 = 0; i2 < saveFields.Count; i2 ++)
				{
					FieldInfo field = saveFields[i2];
					SaveAndLoadValueAttribute saveAndLoadValue = Attribute.GetCustomAttribute(field, typeof(SaveAndLoadValueAttribute)) as SaveAndLoadValueAttribute;
					if (saveAndLoadValue == null)
					{
						saveFields.RemoveAt(i2);
						i2 --;
					}
				}
				saveEntry.fields = saveFields.ToArray();
				saveEntries[i] = saveEntry;
			}
		// 	SaveAndLoadManager.saveAndLoadObjectTypeDict.Add(typeId, this);
		// }
		// else
		// {
		// 	saveEntries = sameTypeObj.saveEntries;
		// 	SaveEntry saveEntry;
		// 	for (int i = 0; i < saveEntries.Length; i ++)
		// 	{
		// 		saveEntry = saveEntries[i];
		// 		saveEntry.saveableAndLoadable = saveables[i];
		// 		saveEntry.saveableAndLoadObject = this;
		// 	}
		// }
	}
}
