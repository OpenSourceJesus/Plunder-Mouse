#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Extensions;

namespace PlunderMouse
{
	[CustomEditor(typeof(EnemyGroup))]
	[CanEditMultipleObjects]
	public class EnemyGroupEditor : Editor
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
			EnemyGroup enemyGroup = (EnemyGroup) target;
			enemyGroup.enemies = enemyGroup.GetComponentsInChildren<Enemy>();
			if (enemyGroup.useCustomYPosition)
			{
				Transform child = enemyGroup.trs.GetChild(0);
				Bounds bounds = new Bounds();
				bounds.SetMinMax(child.position, child.position);
				for (int i = 1; i < enemyGroup.trs.childCount; i ++)
				{
					child = enemyGroup.trs.GetChild(i);
					if (enemyGroup.trs.TransformPoint(child.localPosition).x < bounds.min.x)
						bounds.min = bounds.min.SetX(enemyGroup.trs.TransformPoint(child.localPosition).x);
					if (enemyGroup.trs.TransformPoint(child.localPosition).x > bounds.max.x)
						bounds.max = bounds.max.SetX(enemyGroup.trs.TransformPoint(child.localPosition).x);
					if (enemyGroup.trs.TransformPoint(child.localPosition).z < bounds.min.z)
						bounds.min = bounds.min.SetZ(enemyGroup.trs.TransformPoint(child.localPosition).z);
					if (enemyGroup.trs.TransformPoint(child.localPosition).z > bounds.max.z)
						bounds.max = bounds.max.SetZ(enemyGroup.trs.TransformPoint(child.localPosition).z);
				}
				Vector3 previousPosition = enemyGroup.trs.position;
				enemyGroup.trs.position = bounds.center.SetY(enemyGroup.yPosition);
				foreach (Enemy enemy in enemyGroup.enemies)
					enemy.trs.position += previousPosition - enemyGroup.trs.position;
			}
		}
	}
}
#endif