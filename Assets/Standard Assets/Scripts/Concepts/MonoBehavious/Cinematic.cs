using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using PlunderMouse;
using UnityEngine.InputSystem;
using Extensions;
using Unity.XR.Oculus.Input;
using GameDevJourney;

public class Cinematic : SingletonMonoBehaviour<Cinematic>, IUpdatable
{
	public bool PauseWhileUnfocused
	{
		get
		{
			return true;
		}
	}
	public VideoPlayer video;
	public string loadLevelOnDone;
	public Animation[] skipNotifyAnims;
	public float skipAfterTime;
	public float playSkipNotifyAnimsInterval;
	float skipTimer;
	InputAction anyInputAction;

	void OnEnable ()
	{
		anyInputAction = new InputAction(binding: "/*/<button>");
		anyInputAction.performed += ShowSkipNotification;
		anyInputAction.Enable();
		GameManager.updatables = GameManager.updatables.Add(this);
	}
	
	public void DoUpdate ()
	{
		if (InputManager.SkipCinematicInput)
			skipTimer += Time.deltaTime;
		else
			skipTimer = 0;
		if (Time.timeSinceLevelLoad > video.frameCount * (1f / video.frameRate) / video.playbackSpeed || skipTimer > skipAfterTime)
		{
			enabled = false;
			LevelManager.Instance.mostRecentLevelName = loadLevelOnDone;
			LevelManager.Instance.LoadLevelWithTransition (loadLevelOnDone);
		}
	}

	void OnDisable ()
	{
		GameManager.updatables = GameManager.updatables.Remove(this);
		anyInputAction.Disable();
		anyInputAction.performed -= ShowSkipNotification;
	}

	void ShowSkipNotification (InputAction.CallbackContext context)
	{
		InputManager.leftTouchController = (OculusTouchController) OculusTouchController.leftHand;
		InputManager.rightTouchController = (OculusTouchController) OculusTouchController.rightHand;
		if (context.control.device != InputManager.hmd || InputManager.leftTouchController.gripPressed.isPressed || InputManager.leftTouchController.trigger.ReadValue() >= GameManager.Instance.minTriggerInputValueToPress || InputManager.leftTouchController.primaryButton.isPressed || InputManager.leftTouchController.secondaryButton.isPressed || InputManager.rightTouchController.gripPressed.isPressed || InputManager.rightTouchController.trigger.ReadValue() >= GameManager.Instance.minTriggerInputValueToPress || InputManager.rightTouchController.primaryButton.isPressed || InputManager.rightTouchController.secondaryButton.isPressed)
		{
			bool isPlayingAnim = false;
			foreach (Animation anim in skipNotifyAnims)
			{
				if (anim.isPlaying)
				{
					isPlayingAnim = true;
					break;
				}
			}
			if (!isPlayingAnim)
			{
				foreach (Animation anim in skipNotifyAnims)
				{
					if (anim.gameObject.activeInHierarchy)
						anim.Play();
				}
			}
		}
		anyInputAction.Disable();
		anyInputAction.performed -= ShowSkipNotification;
		StartCoroutine(DelaySetAnyInputActionRoutine ());
	}

	IEnumerator DelaySetAnyInputActionRoutine ()
	{
		yield return new WaitForSecondsRealtime(playSkipNotifyAnimsInterval);
		anyInputAction = new InputAction(binding: "/*/<button>");
		anyInputAction.performed += ShowSkipNotification;
		anyInputAction.Enable();
	}
}
