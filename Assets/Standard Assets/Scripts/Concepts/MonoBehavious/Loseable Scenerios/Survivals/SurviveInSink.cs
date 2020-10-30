using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

namespace PlunderMouse
{
	public class SurviveInSink : Survival
	{
		public int surviveScoreToWin;

		public override void Lose ()
		{
			if ((int) score > Highscore)
				Highscore = (int) score;
			if (score >= surviveScoreToWin)
			{
				SceneManager.sceneLoaded += OnSceneLoaded;
				GameManager.GetSingleton<LevelManager>().LoadLevelWithTransition ("Cinematic");
			}
			else
				GameManager.GetSingleton<LevelManager>().LoadLevelWithTransition("Game Over");
		}

		public virtual void OnSceneLoaded (Scene scene = new Scene(), LoadSceneMode loadMode = LoadSceneMode.Single)
		{
			GameManager.GetSingleton<QuestManager>().CompleteQuest ("Defend against pirates!");
			GameManager.GetSingleton<SaveAndLoadManager>().Save ();
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}