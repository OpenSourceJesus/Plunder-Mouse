using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;

public class SoundEffect : MonoBehaviour
{
	public Transform trs;
	public AudioSource audioSource;
	public Settings settings = new Settings();
	
	public virtual void Play ()
	{
		audioSource.clip = settings.audioClip;
		audioSource.volume = settings.Volume;
		audioSource.maxDistance = settings.MaxDistance;
		audioSource.minDistance = settings.MinDistance;
		audioSource.Play();
		if (settings.persistant)
			DontDestroyOnLoad(gameObject);
		Destroy(gameObject, audioSource.clip.length);
	}
	
	[Serializable]
	public class Settings
	{
		public AudioClip audioClip;
		public bool persistant;
		float volume = MathfExtensions.NULL_FLOAT;
		public float Volume
		{
			get
			{
				if (volume == MathfExtensions.NULL_FLOAT)
					return GameManager.GetSingleton<AudioManager>().soundEffectPrefab.audioSource.volume;
				else
					return volume;
			}
			set
			{
				volume = value;
			}
		}
		float maxDistance = MathfExtensions.NULL_FLOAT;
		public float MaxDistance
		{
			get
			{
				if (maxDistance == MathfExtensions.NULL_FLOAT)
					return GameManager.GetSingleton<AudioManager>().soundEffectPrefab.audioSource.maxDistance;
				else
					return maxDistance;
			}
			set
			{
				maxDistance = value;
			}
		}
		float minDistance = MathfExtensions.NULL_FLOAT;
		public float MinDistance
		{
			get
			{
				if (minDistance == MathfExtensions.NULL_FLOAT)
					return GameManager.GetSingleton<AudioManager>().soundEffectPrefab.audioSource.minDistance;
				else
					return minDistance;
			}
			set
			{
				minDistance = value;
			}
		}
		public Transform speakerTrs;
		Vector3 position = VectorExtensions.NULL;
		public Vector3 Position
		{
			get
			{
				if (speakerTrs != null)
					return speakerTrs.position;
				else
				{
					if (position == VectorExtensions.NULL)
						return GameManager.GetSingleton<AudioManager>().soundEffectPrefab.trs.position;
					else
						return position;
				}
			}
			set
			{
				position = value;
			}
		}
		Quaternion rotation = QuaternionExtensions.NULL;
		public Quaternion Rotation
		{
			get
			{
				if (speakerTrs != null)
					return speakerTrs.rotation;
				else
				{
					if (rotation == QuaternionExtensions.NULL)
						return GameManager.GetSingleton<AudioManager>().soundEffectPrefab.trs.rotation;
					else
						return rotation;
				}
			}
			set
			{
				rotation = value;
			}
		}
	}
}
