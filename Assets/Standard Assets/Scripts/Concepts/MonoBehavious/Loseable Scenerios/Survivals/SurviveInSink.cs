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
				LevelManager.Instance.LoadLevelWithTransition ("Cinematic");
			}
			else
				LevelManager.Instance.LoadLevelWithTransition("Game Over");
		}

		public virtual void OnSceneLoaded (Scene scene = new Scene(), LoadSceneMode loadMode = LoadSceneMode.Single)
		{
			QuestManager.Instance.CompleteQuest ("Defend against pirates!");
			SaveAndLoadManager.Instance.Save ();
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}