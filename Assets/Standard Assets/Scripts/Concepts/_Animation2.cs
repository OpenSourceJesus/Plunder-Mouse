using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace PlunderMouse
{
	[Serializable]
	public class _Animation2
	{
		public float Duration
		{
			get
			{
				return endTime - startTime;
			}
		}
		public string name;
		public float playRate;
		public float currentTime;
		public float startTime;
		public float endTime;
		public MonoBehaviour alembicStreamPlayer;
		public WrapMode wrapMode;
		[HideInInspector]
		public _Animator2 animator;
		Coroutine playRoutine;
		int playDirection;

		public void Init ()
		{
			alembicStreamPlayer.InvokeMember ("OnEnable", BindingFlags.InvokeMethod);
		}

		public void SetCurrentTime (float time)
		{
			currentTime = time;
			alembicStreamPlayer.SetMember<float> ("CurrentTime", currentTime);
			alembicStreamPlayer.InvokeMember ("Update", BindingFlags.InvokeMethod);
			alembicStreamPlayer.InvokeMember ("LateUpdate", BindingFlags.InvokeMethod);
		}
		
		public virtual void Play (bool playForwards = true)
		{
			if (playForwards)
				playDirection = 1;
			else
				playDirection = -1;
			playRoutine = animator.StartCoroutine(PlayRoutine ());
		}
		
		public virtual void Stop ()
		{
			if (playRoutine != null)
				animator.StopCoroutine(playRoutine);
		}
		
		public IEnumerator PlayRoutine ()
		{
			currentTime = startTime;
			do
			{
				currentTime += playRate * playDirection * Time.deltaTime;
				SetCurrentTime (currentTime);
				if (wrapMode == WrapMode.Once && currentTime >= endTime)
					yield break;
				else if (wrapMode == WrapMode.Loop)
				{
					while (currentTime >= endTime)
						currentTime -= Duration;
				}
				else if (wrapMode == WrapMode.PingPong)
				{
					if (currentTime >= endTime)
					{
						currentTime = endTime + (endTime - currentTime);
						playDirection *= -1;
					}
					else if (currentTime <= startTime)
					{
						currentTime = startTime + (startTime - currentTime);
						playDirection *= -1;
					}
				}
				yield return new WaitForEndOfFrame();
			} while (true);
		}
		
		public enum WrapMode
		{
			Once,
			Loop,
			PingPong,
		}
	}
}