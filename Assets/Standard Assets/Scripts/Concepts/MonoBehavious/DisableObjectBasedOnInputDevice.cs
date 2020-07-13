using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlunderMouse
{
	public class DisableObjectBasedOnInputDevice : MonoBehaviour
	{
		public bool disableIfUsing;
		public InputManager.InputDevice inputDevice;
		
		public virtual void Start ()
		{
			gameObject.SetActive(GameManager.GetSingleton<InputManager>().inputDevice == inputDevice != disableIfUsing);
		}
	}
}