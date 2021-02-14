using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class WavesAnimation : MonoBehaviour, IUpdatable
{
	public float waveMovmentMultiplier;
	public Vector2 waveRate;
	public Vector2 waveOffset;
	public Material material;
	public Transform wavesAudioTrs;
	public float audioMovmentMultiplier;
	
	public virtual void Start ()
	{
		GameManager.updatables = GameManager.updatables.Add(this);
	}

	public virtual void DoUpdate ()
	{
		material.mainTextureOffset = new Vector2(Mathf.Sin(Time.time * waveRate.x) + waveOffset.x, Mathf.Sin(Time.time * waveRate.y) + waveOffset.y) * waveMovmentMultiplier;
		if (wavesAudioTrs != null)
			wavesAudioTrs.position = OVRCameraRig.Instance.eyesTrs.position + (new Vector2(material.mainTexture.width, material.mainTexture.height).Multiply(material.mainTextureOffset) * audioMovmentMultiplier).XYToXZ();
	}
	
	public virtual void OnDestroy ()
	{
		material.mainTextureOffset = Vector2.zero;
		GameManager.updatables = GameManager.updatables.Remove(this);
	}
}
