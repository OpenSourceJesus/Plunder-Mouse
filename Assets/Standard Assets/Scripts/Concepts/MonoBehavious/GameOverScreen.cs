using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using GameDevJourney;

public class GameOverScreen : SingletonMonoBehaviour<GameOverScreen>
{
	public virtual void GameOver ()
	{
		LevelManager.Instance.LoadLevelWithTransition (LevelManager.Instance.mostRecentLevelName);
	}
}
