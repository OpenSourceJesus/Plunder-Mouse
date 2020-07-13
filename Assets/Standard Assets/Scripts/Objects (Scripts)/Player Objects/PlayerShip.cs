using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlunderMouse;

public class PlayerShip : PlayerObject
{
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
		GameManager.GetSingleton<PlayerMouse>().trs.localPosition = GameManager.GetSingleton<PlayerMouse>().positionOffsetFromShip;
		HandleAttacking ();
		// canSwitch = switchIndicatorTrigger.collidersInside.Count > 0;
		// switchIndicator.SetActive(canSwitch);
		// if (canSwitch && (OVRInput.GetDown(OVRInput.Button.Three) || InputManager.inputter.GetButtonDown("Interact")))
		// 	GameManager.GetSingleton<PlayerMouse>().DockShip();
	}
	
	public virtual void HandleAttacking ()
	{
		weapon.trs.parent.rotation = OVRCameraRig.CurrentHand.rotation;
		if (InputManager.leftTouchController.trigger.ReadValue() >= GameManager.GetSingleton<GameManager>().minTriggerInputValueToPress || InputManager.rightTouchController.trigger.ReadValue() >= GameManager.GetSingleton<GameManager>().minTriggerInputValueToPress)
			Attack ();
	}
	
	public virtual void Attack ()
	{
		weapon.Attack ();
	}
}
