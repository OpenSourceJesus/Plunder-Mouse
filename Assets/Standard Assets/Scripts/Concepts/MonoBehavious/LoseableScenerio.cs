using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoseableScenerio : MonoBehaviour
{
	public static List<LoseableScenerio> activeScenarios = new List<LoseableScenerio>();
	
	public virtual void OnEnable ()
	{
		activeScenarios.Add(this);
	}
	
	public virtual void OnDisable ()
	{
		activeScenarios.Remove(this);
	}
	
	public virtual void Lose ()
	{
		GameManager.GetSingleton<LevelManager>().LoadLevelWithTransition("Game Over");
	}
}
