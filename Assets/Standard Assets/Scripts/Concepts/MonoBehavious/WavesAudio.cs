using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesAudio : MonoBehaviour
{
	public SoundEffect[] soundEffects;
	
	public virtual void Start ()
	{
		foreach (SoundEffect soundEffect in soundEffects)
		{
			soundEffect.audioSource.minDistance *= soundEffect.trs.lossyScale.x;
			soundEffect.audioSource.maxDistance *= soundEffect.trs.lossyScale.x;
		}
	}
}
