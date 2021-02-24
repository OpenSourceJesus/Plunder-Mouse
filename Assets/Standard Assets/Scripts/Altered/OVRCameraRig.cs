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
	public float lookRate;
	public LayerMask whatICollideWith;
	[HideInInspector]
	public float cameraDistance;
	Vector3 positionOffset;
	Quaternion rota;
	bool setOrientationInput;
	bool previousSetOrientationInput;
	
	void Start ()
	{
		CurrentHand = rightHandTrs;
		cameraDistance = trs.localPosition.magnitude;
		positionOffset = trs.localPosition;
		trs.SetParent(null);
		SetOrientation ();
		GameManager.updatables = GameManager.updatables.Add(this);
	}

	public void DoUpdate ()
	{
		if (InputManager.Instance.inputDevice == InputManager.InputDevice.OculusRift)
			UpdateAnchors ();
		else
		{
			Vector2 rotaInput = Mouse.current.delta.ReadValue().FlipY() * lookRate * Time.deltaTime;
			if (rotaInput != Vector2.zero)
			{
				trackingSpaceTrs.RotateAround(trackingSpaceTrs.position, trackingSpaceTrs.right, rotaInput.y);
				trackingSpaceTrs.RotateAround(trackingSpaceTrs.position, Vector3.up, rotaInput.x);
			}
		}
		setOrientationInput = InputManager.SetOrientationInput;
		if (setOrientationInput && !previousSetOrientationInput)
			SetOrientation ();
		previousSetOrientationInput = setOrientationInput;
	}

	void OnDestroy ()
	{
		GameManager.updatables = GameManager.updatables.Remove(this);
	}

	void UpdateAnchors ()
	{
		if (PlayerObject.CurrentActive != null)
		{
			RaycastHit hit;
			if (Physics.Raycast(PlayerObject.CurrentActive.trs.position, trs.position - PlayerObject.CurrentActive.trs.position, out hit, cameraDistance, whatICollideWith))
				positionOffset = positionOffset.normalized * hit.distance;
			else
				positionOffset = positionOffset.normalized * cameraDistance;
			trs.position = PlayerObject.CurrentActive.trs.position + (rota * positionOffset);
		}
		eyesTrs.localPosition = InputManager.HeadPosition;
		eyesTrs.localRotation = InputManager.HeadRotation;
		leftHandTrs.localRotation = InputManager.LeftHandRotation;
		rightHandTrs.localRotation = InputManager.RightHandRotation;
		leftHandTrs.localPosition = InputManager.LeftHandPosition;
		rightHandTrs.localPosition = InputManager.RightHandPosition;
	}
	
	void SetOrientation ()
	{
		rota = eyesTrs.rotation;
		trackingSpaceTrs.forward = eyesTrs.forward.GetXZ();
	}
}