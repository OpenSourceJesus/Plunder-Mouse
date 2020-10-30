#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

// [ExecuteInEditMode]
public class TerrainObject : MonoBehaviour
{
	public Transform trs;
	public bool autoSetRotation;

	void OnValidate ()
	{
		if (autoSetRotation)
			trs.rotation = Quaternion.LookRotation(Vector3.forward, GameManager.GetSingleton<Terrain>().terrainData.GetInterpolatedNormal(1f / (GameManager.GetSingleton<Terrain>().terrainData.size.x / trs.position.x) + .5f, 1f / (GameManager.GetSingleton<Terrain>().terrainData.size.z / trs.position.z) + .5f));
		trs.position = trs.position.SetY(GameManager.GetSingleton<Terrain>().GetComponent<Transform>().position.y + GameManager.GetSingleton<Terrain>().terrainData.GetInterpolatedHeight(1f / (GameManager.GetSingleton<Terrain>().terrainData.size.x / trs.position.x) + .5f, 1f / (GameManager.GetSingleton<Terrain>().terrainData.size.z / trs.position.z) + .5f));
	}
}
#endif