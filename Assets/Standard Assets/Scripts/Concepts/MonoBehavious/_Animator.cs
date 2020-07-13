using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WrapMode = PlunderMouse._Animation.WrapMode;

namespace PlunderMouse
{
	public class _Animator : MonoBehaviour
	{
		public _Animation[] animations = new _Animation[0];
		List<_Animation> currentlyPlayingAnimations = new List<_Animation>();
		public Dictionary<string, _Animation> animationDict = new Dictionary<string, _Animation>();
		
		public virtual void Init ()
		{
			foreach (_Animation animation in animations)
			{
				animation.animator = this;
				animationDict.Add(animation.name, animation);
			}
		}
		
		public virtual void Play (_Animation animation, int startFrameIndex = 0, bool playForwards = true)
		{
			animation.Play (startFrameIndex, playForwards);
			currentlyPlayingAnimations.Add(animation);
		}
		
		public virtual void Play (string animationName, int startFrameIndex = 0, bool playForwards = true)
		{
			Play (animationDict[animationName], startFrameIndex, playForwards);
		}
		
		public virtual void Play (int animationIndex, int startFrameIndex = 0, bool playForwards = true)
		{
			Play (animations[animationIndex], startFrameIndex, playForwards);
		}

		public virtual void StopAll (bool clear = false)
		{
			while (currentlyPlayingAnimations.Count > 0)
				Stop (currentlyPlayingAnimations[0], clear);
		}
		
		public virtual void Stop (bool clear = false)
		{
			if (currentlyPlayingAnimations.Count > 0)
				Stop (currentlyPlayingAnimations[0], clear);
		}
		
		public virtual void Stop (_Animation animation, bool clear = false)
		{
			animation.Stop (clear);
			currentlyPlayingAnimations.Remove(animation);
		}
		
		public virtual void Stop (string animationName, bool clear = false)
		{
			Stop (animationDict[animationName], clear);
		}
		
		public virtual void Stop (int animationIndex, bool clear = false)
		{
			Stop (animations[animationIndex], clear);
		}
		
		public virtual List<_Animation> GetCurrentlyPlayingAnimations ()
		{
			return currentlyPlayingAnimations;
		}
		
		public virtual List<string> GetCurrentlyPlayingAnimationNames ()
		{
			List<string> output = new List<string>();
			foreach (_Animation animation in currentlyPlayingAnimations)
				output.Add(animation.name);
			return output;
		}

		public virtual void OnDisable ()
		{
			StopAllCoroutines();
		}
	}
}