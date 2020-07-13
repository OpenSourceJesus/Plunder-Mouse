using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TemporaryDisplayObject
{
	public GameObject obj;
	public float duration;
	public bool realtime;
	
	public virtual IEnumerator DisplayRoutine ()
	{
		obj.SetActive(true);
		if (realtime)
			yield return new WaitForSecondsRealtime(duration);
		else
			yield return new WaitForSeconds(duration);
		obj.SetActive(false);
		yield break;
	}
}