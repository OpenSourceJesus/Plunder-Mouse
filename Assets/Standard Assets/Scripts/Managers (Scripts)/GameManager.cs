using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlunderMouse;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	public static bool paused;
	public GameObject[] registeredGos = new GameObject[0];
	[SaveAndLoadValue]
	static string enabledGosString = "";
	[SaveAndLoadValue]
	static string disabledGosString = "";
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}
	public int uniqueId;
	public int UniqueId
	{
		get
		{
			return uniqueId;
		}
		set
		{
			uniqueId = value;
		}
	}
	public static IUpdatable[] updatables = new IUpdatable[0];
	// public static Dictionary<Type, object> singletons = new Dictionary<Type, object>();
	public float minTriggerInputValueToPress;
	public static int framesSinceLevelLoaded;

	public override void Awake ()
	{
		base.Awake ();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public virtual void OnDestroy ()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	
	public virtual void OnSceneLoaded (Scene scene = new Scene(), LoadSceneMode loadMode = LoadSceneMode.Single)
	{
		StopAllCoroutines();
		framesSinceLevelLoaded = 0;
	}

	public virtual void Update ()
	{
		Physics.Simulate(Time.deltaTime);
		foreach (IUpdatable updatable in updatables)
			updatable.DoUpdate ();
		if (ObjectPool.Instance != null && ObjectPool.Instance.enabled)
			ObjectPool.Instance.DoUpdate ();
		InputSystem.Update ();
		framesSinceLevelLoaded ++;
	}

	// public static T GetSingleton<T> ()
	// {
	// 	if (!singletons.ContainsKey(typeof(T)))
	// 		return GetSingleton<T>(FindObjectsOfType<Object>());
	// 	else
	// 	{
	// 		if (singletons[typeof(T)] == null || singletons[typeof(T)].Equals(default(T)))
	// 		{
	// 			T singleton = GetSingleton<T>(FindObjectsOfType<Object>());
	// 			singletons[typeof(T)] = singleton;
	// 			return singleton;
	// 		}
	// 		else
	// 			return (T) singletons[typeof(T)];
	// 	}
	// }

	// public static T GetSingleton<T> (Object[] objects)
	// {
	// 	if (typeof(T).IsSubclassOf(typeof(Object)))
	// 	{
	// 		foreach (Object obj in objects)
	// 		{
	// 			if (obj is T)
	// 			{
	// 				singletons.Remove(typeof(T));
	// 				singletons.Add(typeof(T), obj);
	// 				break;
	// 			}
	// 		}
	// 	}
	// 	if (singletons.ContainsKey(typeof(T)))
	// 		return (T) singletons[typeof(T)];
	// 	else
	// 		return default(T);
	// }

	// public static T GetSingletonIncludingAssets<T> ()
	// {
	// 	if (!singletons.ContainsKey(typeof(T)))
	// 		return GetSingletonIncludingAssets<T>(FindObjectsOfTypeIncludingAssets(typeof(T)));
	// 	else
	// 	{
	// 		if (singletons[typeof(T)] == null || singletons[typeof(T)].Equals(default(T)))
	// 		{
	// 			T singleton = GetSingleton<T>(FindObjectsOfTypeIncludingAssets(typeof(T)));
	// 			singletons[typeof(T)] = singleton;
	// 			return singleton;
	// 		}
	// 		else
	// 			return (T) singletons[typeof(T)];
	// 	}
	// }

	// public static T GetSingletonIncludingAssets<T> (Object[] objects)
	// {
	// 	if (typeof(T).IsSubclassOf(typeof(Object)))
	// 	{
	// 		foreach (Object obj in objects)
	// 		{
	// 			if (obj is T)
	// 			{
	// 				singletons.Remove(typeof(T));
	// 				singletons.Add(typeof(T), obj);
	// 				break;
	// 			}
	// 		}
	// 	}
	// 	if (singletons.ContainsKey(typeof(T)))
	// 		return (T) singletons[typeof(T)];
	// 	else
	// 		return default(T);
	// }
	
	public virtual void Quit ()
	{
		Application.Quit();
	}

	public static void Log (object obj)
	{
		print(obj);
	}

	// public virtual void OnApplicationQuit ()
	// {
	// 	PlayerPrefs.DeleteAll();
	// }
}
