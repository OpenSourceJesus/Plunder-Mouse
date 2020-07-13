using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Events;

namespace PlunderMouse
{
	public class Survival : LoseableScenerio, IUpdatable
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
		public float score;
		public Text scoreText;
		public Text bestScoreText;
		public float minEnemySpawnDist;
		public List<SpawnEntry> spawnEntries = new List<SpawnEntry>();
		public List<EventEntry> eventEntries = new List<EventEntry>();
		public int Highscore
		{
			get
			{
				return PlayerPrefs.GetInt(LevelManager.CurrentScene.name + " highscore", 0);
			}
			set
			{
				PlayerPrefs.SetInt(LevelManager.CurrentScene.name + " highscore", value);
			}
		}
		public float scorePerSecond;
		public BoxCollider spawnBoundsCollider;
		[HideInInspector]
		public Rect spawnBoundsRect;
		[HideInInspector]
		public float spawnBoundsPerimeter;
		
		public virtual void Start ()
		{
			spawnBoundsRect = Rect.MinMaxRect(spawnBoundsCollider.bounds.min.x, spawnBoundsCollider.bounds.min.z, spawnBoundsCollider.bounds.max.x, spawnBoundsCollider.bounds.max.z);
			spawnBoundsPerimeter = spawnBoundsRect.GetPerimeter();
			bestScoreText.text = "Highscore: " + Highscore;
			StartCoroutine(AddScoreOverTime (scorePerSecond));
			GameManager.updatables = GameManager.updatables.Add(this);
		}

		public virtual void OnDestroy ()
		{
			GameManager.updatables = GameManager.updatables.Remove(this);
		}
		
		public virtual void DoUpdate ()
		{
			for (int i = 0; i < spawnEntries.Count; i ++)
			{
				SpawnEntry entry = spawnEntries[i];
				if (entry.runTimes > 0)
				{
					entry.spawnTimer += Time.deltaTime;
					if (entry.spawnTimer > entry.spawnRate)
					{
						for (int i2 = 0; i2 < entry.spawnAmount; i2 ++)
						{
							entry.spawnTimer = 0;
							float distanceAroundPerimeter;
							Vector3 spawnPosition;
							do
							{
								distanceAroundPerimeter = spawnBoundsPerimeter * Random.value;
								spawnPosition = spawnBoundsRect.GetPointOnEdges(distanceAroundPerimeter).XYToXZ();
								spawnPosition.y = PlayerObject.CurrentActive.trs.position.y;
							} while (Vector3.Distance(PlayerObject.CurrentActive.trs.position, spawnPosition) < minEnemySpawnDist);
							spawnPosition.y = entry.enemyPrefab.trs.position.y;
							Enemy enemy = GameManager.GetSingleton<ObjectPool>().SpawnComponent<Enemy>(entry.enemyPrefab, spawnPosition, Quaternion.identity);
							enemy.Awaken ();
						}
						entry.spawnAmount += entry.spawnAmountIncreaseRate;
						entry.runTimes --;
					}
					spawnEntries[i] = entry;
				}
				else
				{
					spawnEntries.RemoveAt(i);
					i --;
				}
			}
			for (int i = 0; i < eventEntries.Count; i ++)
			{
				EventEntry entry = eventEntries[i];
				if (entry.runTimes > 0)
				{
					entry.triggerTimer += Time.deltaTime;
					if (entry.triggerTimer > entry.triggerTime)
					{
						if (entry.loopTimer)
							entry.triggerTimer = 0;
						entry.unityEvent.Invoke();
						entry.runTimes --;
					}
					eventEntries[i] = entry;
				}
				else
				{
					eventEntries.RemoveAt(i);
					i --;
				}
			}
		}
		
		public virtual IEnumerator AddScoreOverTime (float scorePerSecond)
		{
			while (true)
			{
				yield return new WaitForSeconds(1f / scorePerSecond);
				AddScore ();
			}
		}
		
		public override void Lose ()
		{
			if ((int) score > Highscore)
				Highscore = (int) score;
			base.Lose ();
		}
		
		public virtual void AddScore (float amount = 1)
		{
			score += amount;
			scoreText.text = "Score: " + (int) score;
		}
		
		[Serializable]
		public class SpawnEntry
		{
			public Enemy enemyPrefab;
			public float spawnRate;
			public float spawnTimer;
			[Tooltip("spawnAmountIncreaseRate")]
			public float spawnAmountIncreaseRate;
			public int runTimes;
			public float spawnAmount;
		}
		
		[Serializable]
		public class EventEntry
		{
			public UnityEvent unityEvent;
			public float triggerTime;
			public float triggerTimer;
			public bool loopTimer;
			public int runTimes;
		}
	}
}