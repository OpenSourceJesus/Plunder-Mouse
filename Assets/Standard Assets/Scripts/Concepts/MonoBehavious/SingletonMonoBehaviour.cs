using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlunderMouse;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	public MultipleInstancesHandlingType handleMultipleInstances;
	public bool persistant;
	
	public virtual void Awake ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		if (handleMultipleInstances != MultipleInstancesHandlingType.KeepAll && GameManager.GetSingleton<T>() != null && GameManager.GetSingleton<T>() != this)
		{
			if (handleMultipleInstances == MultipleInstancesHandlingType.DestroyNew)
			{
				Destroy(gameObject);
				return;
			}
			else
				Destroy(GameManager.GetSingleton<T>().gameObject);
		}
		if (persistant)
			DontDestroyOnLoad(gameObject);
	}

	public enum MultipleInstancesHandlingType
	{
		KeepAll,
		DestroyNew,
		DestroyOld
	}
}