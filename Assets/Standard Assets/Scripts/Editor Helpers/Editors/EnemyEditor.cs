#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Extensions;

namespace PlunderMouse
{
	[CustomEditor(typeof(Enemy), true)]
	[CanEditMultipleObjects]
	public class EnemyEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			Update ();
		}

		public virtual void OnSceneGUI ()
		{
			Update ();
		}

		public virtual void Update ()
		{
			if (Application.isPlaying)
				return;
			Enemy enemy = (Enemy) target;
			if (enemy.rigid.useGravity)
			{
				Terrain terrain = FindObjectOfType<Terrain>();
				enemy.trs.position = enemy.trs.position.SetY(terrain.GetComponent<Transform>().position.y + terrain.terrainData.GetInterpolatedHeight(1f / (terrain.terrainData.size.x / enemy.trs.position.x) + .5f, 1f / (terrain.terrainData.size.z / enemy.trs.position.z) + .5f));
			}
			enemy.enemyGroup = enemy.GetComponentInParent<EnemyGroup>();
		}
	}
}
#endif