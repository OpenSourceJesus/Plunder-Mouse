using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlunderMouse;

public class PlayerShip : PlayerObject
{
	public new static PlayerShip instance;
	public new static PlayerShip Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<PlayerShip>();
			return instance;
		}
	}
	
	public override void Start ()
	{
		base.Start();
		switchIndicator.SetActive(false);
		if (!Active)
			switchIndicatorTrigger.gameObject.SetActive(false);
	}
	
	public override void DoUpdate ()
	{
		base.DoUpdate ();
		if (dead || !Active)
			return;
		PlayerMouse.Instance.trs.localPosition = PlayerMouse.Instance.positionOffsetFromShip;
		HandleAttacking ();
		// canSwitch = switchIndicatorTrigger.collidersInside.Count > 0;
		// switchIndicator.SetActive(canSwitch);
		// if (canSwitch && (OVRInput.GetDown(OVRInput.Button.Three) || InputManager.inputter.GetButtonDown("Interact")))
		// 	PlayerMouse.Instance.DockShip();
	}
	
	public virtual void HandleAttacking ()
	{
		weapon.trs.parent.rotation = OVRCameraRig.CurrentHand.rotation;
		if (InputManager._InputDevice == InputManager.InputDevice.KeyboardAndMouse)
		{
			if (InputManager.LeftClickInput)
			{
				Attack ();
			}
		}
		else if (InputManager.LeftTouchController.trigger.ReadValue() >= InputManager.Settings.defaultDeadzoneMin || InputManager.RightTouchController.trigger.ReadValue() >= InputManager.Settings.defaultDeadzoneMin)
			Attack ();
	}
	
	public virtual void Attack ()
	{
		weapon.Attack ();
	}
}
