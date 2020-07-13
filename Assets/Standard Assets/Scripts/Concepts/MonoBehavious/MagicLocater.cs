using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class MagicLocater : MonoBehaviour
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