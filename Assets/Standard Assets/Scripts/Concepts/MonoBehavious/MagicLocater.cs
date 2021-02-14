using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevJourney;

namespace PlunderMouse
{
	public class MagicLocater : SingletonMonoBehaviour<MagicLocater>
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
		public Transform trs;
		public float range;
	}
}