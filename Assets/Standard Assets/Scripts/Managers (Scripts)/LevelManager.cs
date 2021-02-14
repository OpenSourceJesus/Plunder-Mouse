using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameDevJourney;

public class LevelManager : SingletonMonoBehaviour<LevelManager>, ISavableAndLoadable
{
	public static bool isLoading;
	public float transitionRate;
	[SaveAndLoadValue]
	public string mostRecentLevelName;
	public static Scene CurrentScene
	{
		get
		{
			return SceneManager.GetActiveScene();
		}
	}
	
	public void LoadLevelWithTransition (string levelName)
	{
		if (LevelManager.Instance != this)
		{
			LevelManager.Instance.LoadLevelWithTransition (levelName);
			return;
		}
		isLoading = true;
		Time.timeScale = 1;
		StartCoroutine (LevelTransition (levelName));
	}
	
	public void LoadLevelWithoutTransition (string levelName)
	{
		isLoading = true;
		Time.timeScale = 1;
		SceneManager.LoadScene(levelName);
	}
	
	public void LoadLevelWithTransition (int levelId)
	{
		if (LevelManager.Instance != this)
		{
			LevelManager.Instance.LoadLevelWithTransition (levelId);
			return;
		}
		isLoading = true;
		Time.timeScale = 1;
		StartCoroutine (LevelTransition (levelId));
	}
	
	public void LoadLevelWithoutTransition (int levelId)
	{
		isLoading = true;
		Time.timeScale = 1;
		SceneManager.LoadScene(levelId);
	}
	
	public void LoadLevelAdditiveWithTransition (string levelName)
	{
		if (LevelManager.Instance != this)
		{
			LevelManager.Instance.LoadLevelAdditiveWithTransition (levelName);
			return;
		}
		isLoading = true;
		Time.timeScale = 1;
		StartCoroutine (LevelTransition (levelName, LoadSceneMode.Additive));
	}
	
	public void LoadLevelAdditiveWithoutTransition (string levelName)
	{
		isLoading = true;
		Time.timeScale = 1;
		SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
	}
	
	public void RestartLevelWithTransition ()
	{
		isLoading = true;
		LoadLevelWithTransition (SceneManager.GetActiveScene().name);
	}
	
	public void RestartLevelWithoutTransition ()
	{
		isLoading = true;
		LoadLevelWithoutTransition (SceneManager.GetActiveScene().name);
	}
	
	public void NextLevelWithTransition ()
	{
		isLoading = true;
		LoadLevelWithTransition (SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void NextLevelWithoutTransition ()
	{
		isLoading = true;
		LoadLevelWithoutTransition (SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public virtual void OnLevelLoaded (Scene scene = new Scene(), LoadSceneMode loadMode = LoadSceneMode.Single)
	{
		Camera.main.rect = new Rect(.5f, .5f, 0, 0);
		StartCoroutine(LevelTransition (null));
		SceneManager.sceneLoaded -= OnLevelLoaded;
		isLoading = false;
	}
	
	public virtual IEnumerator LevelTransition (string levelName = null, LoadSceneMode loadMode = LoadSceneMode.Single)
	{
		bool transitioningIn = string.IsNullOrEmpty(levelName);
		float transitionRateMultiplier = 1;
		if (transitioningIn)
			transitionRateMultiplier *= -1;
		while ((Camera.main.rect.size.x > 0 && !transitioningIn) || (Camera.main.rect.size.x < 1 && transitioningIn))
		{
			Rect cameraRect = Camera.main.rect;
			cameraRect.size -= Vector2.one * transitionRate * transitionRateMultiplier * Time.unscaledDeltaTime;
			cameraRect.center += Vector2.one * transitionRate * transitionRateMultiplier * Time.unscaledDeltaTime / 2;
			Camera.main.rect = cameraRect;
			yield return new WaitForEndOfFrame();
		}
		if (transitioningIn)
			Camera.main.rect = new Rect(0, 0, 1, 1);
		else
		{
			Camera.main.rect = new Rect(.5f, .5f, 0, 0);
			SceneManager.sceneLoaded += OnLevelLoaded;
			if (!string.IsNullOrEmpty(levelName))
				SceneManager.LoadScene(levelName, loadMode);
		}
	}

	public virtual IEnumerator LevelTransition (int levelId = -1, LoadSceneMode loadMode = LoadSceneMode.Single)
	{
		yield return StartCoroutine(LevelTransition (SceneManager.GetSceneByBuildIndex(levelId).name, loadMode));
	}
}
