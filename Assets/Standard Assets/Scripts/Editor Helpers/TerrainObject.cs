#if UNITY_EDITOR
using UnityEngine;
using Extensions;

[ExecuteInEditMode]
public class TerrainObject : MonoBehaviour
{
	public Transform trs;
	public bool autoSetRotation;

	void OnEnable ()
	{
		Terrain terrain = FindObjectOfType<Terrain>();
		if (autoSetRotation)
			trs.rotation = Quaternion.LookRotation(Vector3.forward, terrain.terrainData.GetInterpolatedNormal(1f / (terrain.terrainData.size.x / trs.position.x) + .5f, 1f / (terrain.terrainData.size.z / trs.position.z) + .5f));
		trs.position = trs.position.SetY(terrain.GetComponent<Transform>().position.y + terrain.terrainData.GetInterpolatedHeight(1f / (terrain.terrainData.size.x / trs.position.x) + .5f, 1f / (terrain.terrainData.size.z / trs.position.z) + .5f));
	}
}
#endif