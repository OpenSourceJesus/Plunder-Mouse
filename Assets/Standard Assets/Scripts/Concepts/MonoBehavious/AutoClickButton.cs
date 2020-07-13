using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using Hotkey = PlunderMouse.InputManager.Hotkey;
using HotkeyState = PlunderMouse.InputManager.HotkeyState;

namespace PlunderMouse
{
	public class AutoClickButton : MonoBehaviour
	{
		public Button button;
		public int initTriggersUntilClick = 1;
		int triggersUntilClick;
		public bool onEnable;
		public bool onAwake;
		public bool onStart;
		public bool onUpdate;
		public bool onDisable;
		public bool onDestroy;
		public bool onLevelLoaded;
		public bool onLevelUnloaded;
		public bool onTriggerEnter2D;
		public bool onTriggerExit2D;
		// public Hotkey[] hotkeys = new Hotkey[0];
		bool hotkeyIsPressed;
		bool justRanAwake;
		
	    void OnEnable ()
		{
			if (onEnable)
	        	Trigger ();
		}
		
		void Awake ()
		{
			triggersUntilClick = initTriggersUntilClick;
			if (onAwake)
				Trigger ();
			if (onLevelLoaded)
				SceneManager.sceneLoaded += LevelLoaded;
			if (onLevelUnloaded)
				SceneManager.sceneUnloaded += LevelUnloaded;
			justRanAwake = true;
		}
		
		void Start ()
		{
			if (onStart)
				Trigger ();
			if (justRanAwake)
				return;
			if (onLevelLoaded)
				SceneManager.sceneLoaded += LevelLoaded;
			if (onLevelUnloaded)
				SceneManager.sceneUnloaded += LevelUnloaded;
			justRanAwake = false;
		}
		
		void Update ()
		{
			if (onUpdate)
				Trigger ();
			// foreach (Hotkey hotkey in hotkeys)
			// {
			// 	foreach (Hotkey.ButtonEntry buttonEntry in hotkey.requiredButtons)
			// 	{
			// 		switch (buttonEntry.pressState)
			// 		{
			// 			case HotkeyState.Down:
			// 				hotkeyIsPressed = buttonEntry.vrButtonGroup.GetDown();
			// 				break;
			// 			case HotkeyState.Held:
			// 				hotkeyIsPressed = buttonEntry.vrButtonGroup.Get();
			// 				break;
			// 			case HotkeyState.Up:
			// 				hotkeyIsPressed = buttonEntry.vrButtonGroup.GetUp();
			// 				break;
			// 		}
			// 		if (!hotkeyIsPressed)
			// 			break;
			// 	}
			// 	if (hotkeyIsPressed)
			// 	{
			// 		Trigger ();
			// 		return;
			// 	}
			// }
		}
		
		void OnDisable ()
		{
			if (onDisable)
				Trigger ();
		}
		
		void OnDestroy ()
		{
			if (onDestroy)
				Trigger ();
		}
		
		void OnTriggerEnter2D (Collider2D other)
		{
			if (onTriggerEnter2D)
				Trigger ();
		}
		
		void OnTriggerExit2D (Collider2D other)
		{
			if (onTriggerExit2D)
				Trigger ();
		}
		
		void LevelLoaded (Scene scene, LoadSceneMode loadMode)
		{
			if (this != null)
				Trigger ();
		}
		
		void LevelUnloaded (Scene scene)
		{
			if (this != null)
				Trigger ();
		}
		
		public void Trigger ()
		{
			if (!Application.isPlaying || !button.gameObject.activeSelf)
				return;
			triggersUntilClick --;
			if (triggersUntilClick == 0 && button != null)
				button.onClick.Invoke ();
		}
		
		public void Restart (int triggersUntilClick)
		{
			this.triggersUntilClick = triggersUntilClick;
		}
		
		public void Restart ()
		{
			Restart (initTriggersUntilClick);
		}
	}
}