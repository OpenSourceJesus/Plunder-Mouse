using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	public float Volume
	{
		get
		{
			return PlayerPrefs.GetFloat("Volume", 1);
		}
		set
		{
			AudioListener.volume = value;
			PlayerPrefs.SetFloat("Volume", value);
		}
	}
	public bool Mute
	{
		get
		{
			return PlayerPrefsExtensions.GetBool("Mute");
		}
		set
		{
			AudioListener.pause = value;
			PlayerPrefsExtensions.SetBool("Mute", value);
		}
	}
	public SoundEffect soundEffectPrefab;
	public AudioClip[] deathSounds;
	public AudioClip[] deathResponses;
	
	public virtual SoundEffect MakeSoundEffect (SoundEffect.Settings soundEffectSettings)
	{
		SoundEffect output = Instantiate(soundEffectPrefab, soundEffectSettings.Position, soundEffectSettings.Rotation);
		output.settings = soundEffectSettings;
		output.Play();
		return output;
	}
}
