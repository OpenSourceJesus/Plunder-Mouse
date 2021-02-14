using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.XR.Oculus.Input;
using UnityEngine.InputSystem;
using Extensions;
using GameDevJourney;

namespace PlunderMouse
{
	public class InputManager : SingletonMonoBehaviour<InputManager>
	{
		public InputDevice inputDevice;
		public static InputDevice _InputDevice
		{
			get
			{
				return InputManager.Instance.inputDevice;
			}
		}
		public InputSettings settings;
		public static InputSettings Settings
		{
			get
			{
				return InputManager.Instance.settings;
			}
		}
		public static bool UsingGamepad
		{
			get
			{
				return Gamepad.current != null;
			}
		}
		public static bool UsingMouse
		{
			get
			{
				return Mouse.current != null;
			}
		}
		public static bool UsingKeyboard
		{
			get
			{
				return Keyboard.current != null;
			}
		}
		public static bool LeftClickInput
		{
			get
			{
				if (UsingGamepad)
					return false;
				else
					return Mouse.current.leftButton.isPressed;
			}
		}
		public bool _LeftClickInput
		{
			get
			{
				return LeftClickInput;
			}
		}
		public static Vector2 MousePosition
		{
			get
			{
				if (UsingMouse)
					return Mouse.current.position.ToVec2();
				else
					return VectorExtensions.NULL;
			}
		}
		public Vector2 _MousePosition
		{
			get
			{
				return MousePosition;
			}
		}
		public static bool SubmitInput
		{
			get
			{
				if (UsingGamepad)
					return Gamepad.current.aButton.isPressed;
				else
					return Keyboard.current.enterKey.isPressed;// || Mouse.current.leftButton.isPressed;
			}
		}
		public bool _SubmitInput
		{
			get
			{
				return SubmitInput;
			}
		}
		public static bool ReplaceInput
		{
			get
			{
				return leftTouchController.trigger.ReadValue() > Settings.defaultDeadzoneMin || rightTouchController.trigger.ReadValue() > Settings.defaultDeadzoneMin;
			}
		}
		public bool _ReplaceInput
		{
			get
			{
				return ReplaceInput;
			}
		}
		public static Vector2 UIMovementInput
		{
			get
			{
				if (UsingGamepad)
					return Vector2.ClampMagnitude(Gamepad.current.leftStick.ToVec2(), 1);
				else
				{
					int x = 0;
					if (Keyboard.current.dKey.isPressed)
						x ++;
					if (Keyboard.current.aKey.isPressed)
						x --;
					int y = 0;
					if (Keyboard.current.wKey.isPressed)
						y ++;
					if (Keyboard.current.sKey.isPressed)
						y --;
					return Vector2.ClampMagnitude(new Vector2(x, y), 1);
				}
			}
		}
		public Vector2 _UIMovementInput
		{
			get
			{
				return UIMovementInput;
			}
		}
		public static bool SkipCinematicInput
		{
			get
			{
				bool output = false;
				if (_InputDevice != InputDevice.KeyboardAndMouse)
				{
					rightTouchController = (OculusTouchController) OculusTouchController.rightHand;
					output = rightTouchController.primaryButton.isPressed;
				}
				if (UsingKeyboard)
					output |= Keyboard.current.spaceKey.isPressed;
				return output;
			}
		}
		public bool _SkipCinematicInput
		{
			get
			{
				return SkipCinematicInput;
			}
		}
		public static OculusHMD hmd;
		public static OculusTouchController leftTouchController;
		public static OculusTouchController rightTouchController;
		
		// IEnumerator Start ()
		// {
		// 	do
		// 	{
		// 		hmd = InputSystem.GetDevice<OculusHMD>();
		// 		yield return new WaitForEndOfFrame();
		// 	} while (hmd == null);
		// 	do
		// 	{
		// 		leftTouchController = (OculusTouchController) OculusTouchController.leftHand;
		// 		yield return new WaitForEndOfFrame();
		// 	} while (leftTouchController == null);
		// 	do
		// 	{
		// 		rightTouchController = (OculusTouchController) OculusTouchController.rightHand;
		// 		yield return new WaitForEndOfFrame();
		// 	} while (rightTouchController == null);
		// 	yield break;
		// }

		public static float GetAxis (InputControl<float> positiveButton, InputControl<float> negativeButton)
		{
			return positiveButton.ReadValue() - negativeButton.ReadValue();
		}

		public static Vector2 GetAxis2D (InputControl<float> positiveXButton, InputControl<float> negativeXButton, InputControl<float> positiveYButton, InputControl<float> negativeYButton)
		{
			Vector2 output = new Vector2();
			output.x = positiveXButton.ReadValue() - negativeXButton.ReadValue();
			output.y = positiveYButton.ReadValue() - negativeYButton.ReadValue();
			output = Vector2.ClampMagnitude(output, 1);
			return output;
		}
		
		public enum HotkeyState
		{
			Down,
			Held,
			Up
		}
		
		public enum InputDevice
		{
			OculusGo,
			OculusRift,
			KeyboardAndMouse
		}
	}
}