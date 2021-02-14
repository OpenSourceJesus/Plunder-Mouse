using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using UnityEngine.InputSystem;
using Unity.XR.Oculus.Input;
using GameDevJourney;
using PlunderMouse;

public class OVRCameraRig : SingletonMonoBehaviour<OVRCameraRig>, IUpdatable
{
	public bool PauseWhileUnfocused
	{
		get
		{
			return true;
		}
	}
	public new Camera camera;
	public Transform trackingSpaceTrs;
	public Transform eyesTrs;
	public Transform leftHandTrs;
	public Transform rightHandTrs;
	public Transform trs;
	static Transform currentHand;
	public static Transform CurrentHand
	{
		get
		{
			return currentHand;
		}
		set
		{
			currentHand = value;
		}
	}
	Vector3 positionOffset;
	Quaternion rota;
	Vector3 previousTrackingSpaceForward;
	public float lookRate;
	bool wasPreviouslySettingOrienation;
	
	void Start ()
	{
		CurrentHand = rightHandTrs;
		positionOffset = trs.localPosition;
		trs.SetParent(null);
		SetOrientation ();
		GameManager.updatables = GameManager.updatables.Add(this);
	}

	public void DoUpdate ()
	{
		if (PlayerObject.CurrentActive != null)
			trs.position = PlayerObject.CurrentActive.trs.position + (rota * positionOffset);
		Vector2 rotaInput = Mouse.current.delta.ToVec2().FlipY() * lookRate * Time.deltaTime;
		if (rotaInput != Vector2.zero)
		{
			trackingSpaceTrs.RotateAround(trackingSpaceTrs.position, trackingSpaceTrs.right, rotaInput.y);
			trackingSpaceTrs.RotateAround(trackingSpaceTrs.position, Vector3.up, rotaInput.x);
		}
		if (InputManager.Instance.inputDevice == InputManager.InputDevice.OculusRift)
			UpdateAnchors ();
		if ((InputManager.Instance.inputDevice == InputManager.InputDevice.KeyboardAndMouse && Keyboard.current.spaceKey.isPressed) || (InputManager.Instance.inputDevice == InputManager.InputDevice.OculusRift && (InputManager.leftTouchController.gripPressed.isPressed || InputManager.rightTouchController.gripPressed.isPressed)))
		{
			if (!wasPreviouslySettingOrienation)
			{
				wasPreviouslySettingOrienation = true;
				SetOrientation ();
			}
		}
		else
			wasPreviouslySettingOrienation = false;
	}

	void OnDestroy ()
	{
		GameManager.updatables = GameManager.updatables.Remove(this);
	}

	void UpdateAnchors ()
	{
		InputManager.hmd = InputSystem.GetDevice<OculusHMD>();
		eyesTrs.localPosition = InputManager.hmd.devicePosition.ToVec3();
		eyesTrs.localRotation = InputManager.hmd.deviceRotation.ToQuat();
		InputManager.leftTouchController = (OculusTouchController) OculusTouchController.leftHand;
		InputManager.rightTouchController = (OculusTouchController) OculusTouchController.rightHand;
		leftHandTrs.localRotation = InputManager.leftTouchController.deviceRotation.ToQuat();
		rightHandTrs.localRotation = InputManager.rightTouchController.deviceRotation.ToQuat();
		leftHandTrs.localPosition = InputManager.leftTouchController.devicePosition.ToVec3();
		rightHandTrs.localPosition = InputManager.rightTouchController.devicePosition.ToVec3();
	}
	
	void SetOrientation ()
	{
		rota = Quaternion.LookRotation(eyesTrs.forward.GetXZ().SetY(eyesTrs.forward.y), Vector3.up);
		trackingSpaceTrs.forward = eyesTrs.forward.GetXZ();
	}
}