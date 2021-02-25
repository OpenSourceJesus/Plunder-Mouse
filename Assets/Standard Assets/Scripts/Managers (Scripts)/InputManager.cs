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
				return Instance.inputDevice;
			}
		}
		public InputSettings settings;
		public static InputSettings Settings
		{
			get
			{
				return Instance.settings;
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
					return Mouse.current.position.ReadValue();
				else
					return VectorExtensions.NULL3;
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
		public static Vector2 UIMovementInput
		{
			get
			{
				if (UsingGamepad)
					return Vector2.ClampMagnitude(Gamepad.current.leftStick.ReadValue(), 1);
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
				if (_InputDevice != InputDevice.KeyboardAndMouse)
					return RightTouchController.primaryButton.isPressed;
				else
					return Keyboard.current.spaceKey.isPressed;
			}
		}
		public bool _SkipCinematicInput
		{
			get
			{
				return SkipCinematicInput;
			}
		}
		public static Vector2 MoveInput
		{
			get
			{
				Vector2 output = new Vector2();
				if (LeftTouchController != null)
					output = LeftTouchController.thumbstick.ReadValue();
				if (RightTouchController != null)
					output += RightTouchController.thumbstick.ReadValue();
				return Vector2.ClampMagnitude(output, 1);
			}
		}
		public Vector2 _MoveInput
		{
			get
			{
				return MoveInput;
			}
		}
		public static bool SetOrientationInput
		{
			get
			{
				if (_InputDevice == InputDevice.KeyboardAndMouse)
					return Keyboard.current.spaceKey.isPressed;
				else if (_InputDevice == InputDevice.OculusRift)
					return LeftTouchController.gripPressed.isPressed || RightTouchController.gripPressed.isPressed;
				else
					return false;
			}
		}
		public bool _SetOrientationInput
		{
			get
			{
				return SetOrientationInput;
			}
		}
		public static bool JumpInput
		{
			get
			{
				if (_InputDevice == InputDevice.KeyboardAndMouse)
					return Keyboard.current.leftShiftKey.isPressed;
				else if (_InputDevice == InputDevice.OculusRift)
					return LeftTouchController.primaryButton.isPressed || LeftTouchController.secondaryButton.isPressed || RightTouchController.primaryButton.isPressed || RightTouchController.secondaryButton.isPressed;
				else
					return false;
			}
		}
		public bool _JumpInput
		{
			get
			{
				return JumpInput;
			}
		}
		public static bool LeftAttackInput
		{
			get
			{
				if (_InputDevice == InputDevice.KeyboardAndMouse)
					return Mouse.current.leftButton.isPressed;
				else
					return LeftTriggerInput;
			}
		}
		public bool _LeftAttackInput
		{
			get
			{
				return LeftAttackInput;
			}
		}
		public static bool RightAttackInput
		{
			get
			{
				if (_InputDevice == InputDevice.KeyboardAndMouse)
					return Mouse.current.rightButton.isPressed;
				else
					return RightTriggerInput;
			}
		}
		public bool _RightAttackInput
		{
			get
			{
				return RightAttackInput;
			}
		}
		public static bool LeftGripInput
		{
			get
			{
				return LeftTouchController != null && LeftTouchController.gripPressed.isPressed;
			}
		}
		public bool _LeftGripInput
		{
			get
			{
				return LeftGripInput;
			}
		}
		public static bool RightGripInput
		{
			get
			{
				return RightTouchController != null && RightTouchController.gripPressed.isPressed;
			}
		}
		public bool _RightGripInput
		{
			get
			{
				return RightGripInput;
			}
		}
		public static bool LeftTriggerInput
		{
			get
			{
				return LeftTouchController != null && LeftTouchController.triggerPressed.isPressed;
			}
		}
		public bool _LeftTriggerInput
		{
			get
			{
				return LeftTriggerInput;
			}
		}
		public static bool RightTriggerInput
		{
			get
			{
				return RightTouchController != null && RightTouchController.triggerPressed.isPressed;
			}
		}
		public bool _RightTriggerInput
		{
			get
			{
				return RightTriggerInput;
			}
		}
		public static Vector3 HeadPosition
		{
			get
			{
				if (Hmd != null)
					return Hmd.devicePosition.ReadValue();
				else
					return VectorExtensions.NULL3;
			}
		}
		public Vector3 _HeadPosition
		{
			get
			{
				return HeadPosition;
			}
		}
		public static Quaternion HeadRotation
		{
			get
			{
				if (Hmd != null)
					return Hmd.deviceRotation.ReadValue();
				else
					return QuaternionExtensions.NULL;
			}
		}
		public Quaternion _HeadRotation
		{
			get
			{
				return HeadRotation;
			}
		}
		public static Vector3 LeftHandPosition
		{
			get
			{
				if (LeftTouchController != null)
					return LeftTouchController.devicePosition.ReadValue();
				else
					return VectorExtensions.NULL3;
			}
		}
		public Vector3 _LeftHandPosition
		{
			get
			{
				return LeftHandPosition;
			}
		}
		public static Quaternion LeftHandRotation
		{
			get
			{
				if (LeftTouchController != null)
					return LeftTouchController.deviceRotation.ReadValue();
				else
					return QuaternionExtensions.NULL;
			}
		}
		public Quaternion _LeftHandRotation
		{
			get
			{
				return LeftHandRotation;
			}
		}
		public static Vector3 RightHandPosition
		{
			get
			{
				if (RightTouchController != null)
					return RightTouchController.devicePosition.ReadValue();
				else
					return VectorExtensions.NULL3;
			}
		}
		public Vector3 _RightHandPosition
		{
			get
			{
				return RightHandPosition;
			}
		}
		public static Quaternion RightHandRotation
		{
			get
			{
				if (RightTouchController != null)
					return RightTouchController.deviceRotation.ReadValue();
				else
					return QuaternionExtensions.NULL;
			}
		}
		public Quaternion _RightHandRotation
		{
			get
			{
				return RightHandRotation;
			}
		}
		public static OculusHMD Hmd
		{
			get
			{
				return InputSystem.GetDevice<OculusHMD>();
			}
		}
		public static OculusTouchController LeftTouchController
		{
			get
			{
				return (OculusTouchController) OculusTouchController.leftHand;
			}
		}
		public static OculusTouchController RightTouchController
		{
			get
			{
				return (OculusTouchController) OculusTouchController.rightHand;
			}
		}
		
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