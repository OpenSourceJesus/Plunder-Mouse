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
	
	public virtual void Quit ()
	{
		Application.Quit();
	}

	public static void Log (object obj)
	{
		print(obj);
	}

	// void OnApplicationQuit ()
	// {
	// 	PlayerPrefs.DeleteAll();
	// }
}
