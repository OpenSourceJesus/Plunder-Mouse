using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
	public void SetTimeScale (float timeScale)
	{
		Time.timeScale = timeScale;
	}
}
