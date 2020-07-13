using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameOverScreen : MonoBehaviour
{
	public virtual void GameOver ()
	{
		GameManager.GetSingleton<LevelManager>().LoadLevelWithTransition (GameManager.GetSingleton<LevelManager>().mostRecentLevelName);
	}
}
