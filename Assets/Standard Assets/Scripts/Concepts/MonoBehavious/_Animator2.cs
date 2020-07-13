using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FastMemberCore;
using System.Reflection;

namespace PlunderMouse
{
	public class _Animator2 : MonoBehaviour
	{
		public _Animation2[] animations = new _Animation2[0];
		List<_Animation2> currentlyPlayingAnimations = new List<_Animation2>();
		public Dictionary<string, _Animation2> animationDict = new Dictionary<string, _Animation2>();
		
		public virtual void Awake ()
		{
			foreach (_Animation2 animation in animations)
			{
				animation.animator = this;
				animation.Init ();
				animationDict.Add(animation.name, animation);
			}
		}
		
		public virtual void Play (_Animation2 animation, bool playForwards = true)
		{
			animation.Play (playForwards);
			currentlyPlayingAnimations.Add(animation);
		}
		
		public virtual void Play (string animationName, bool playForwards = true)
		{
			Play (animationDict[animationName], playForwards);
		}
		
		public virtual void Play (int animationIndex, bool playForwards = true)
		{
			Play (animations[animationIndex], playForwards);
		}

		public virtual void StopAll ()
		{
			while (currentlyPlayingAnimations.Count > 0)
				Stop (currentlyPlayingAnimations[0]);
		}
		
		public virtual void Stop ()
		{
			if (currentlyPlayingAnimations.Count > 0)
				Stop (currentlyPlayingAnimations[0]);
		}
		
		public virtual void Stop (_Animation2 animation)
		{
			animation.Stop ();
			currentlyPlayingAnimations.Remove(animation);
		}
		
		public virtual void Stop (string animationName)
		{
			Stop (animationDict[animationName]);
		}
		
		public virtual void Stop (int animationIndex)
		{
			Stop (animations[animationIndex]);
		}
		
		public virtual List<_Animation2> GetCurrentlyPlayingAnimations ()
		{
			return currentlyPlayingAnimations;
		}
		
		public virtual List<string> GetCurrentlyPlayingAnimationNames ()
		{
			List<string> output = new List<string>();
			foreach (_Animation2 animation in currentlyPlayingAnimations)
				output.Add(animation.name);
			return output;
		}

		public virtual void OnDisable ()
		{
			StopAllCoroutines();
		}
	}
}