#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
	public bool meld;
	public Transform trs;
	public Terrain terrain;
	public Vector2Int terrainPosition;
	public static List<TerrainGenerator> terrainGenerators = new List<TerrainGenerator>();
	public static List<Vector2Int> terrainPositions = new List<Vector2Int>();
	public TerrainData baseTerrainData;
	public TerrainData terrainData;
	float[,] heights;
	public float hillHeight;
	int width;
	int height;
	Vector2 offset;
	string assetName;
	new TerrainCollider collider;
	public AnimationCurve xMultiplyToHeight;
	public AnimationCurve zMultiplyToHeight;
	public AnimationCurve xAddToHeight;
	public AnimationCurve zAddToHeight;
	float heightInfluence;
	float xNormalized;
	float zNormalized;
	public float perlinNoiseScale;
	public Vector2 perlinNoiseOffset;
	// EditorCoroutine updateRoutine;
	
	public virtual void OnEnable ()
	{
		if (trs == null)
			trs = GetComponent<Transform>();
		terrainPosition = new Vector2Int((int) (trs.position.x / baseTerrainData.size.x), (int) (trs.position.z / baseTerrainData.size.z));
		if (!terrainPositions.Contains(terrainPosition))
		{
			terrainGenerators.Add(this);
			terrainPositions.Add(terrainPosition);
		}
		terrainData = Instantiate(baseTerrainData);
		assetName = "Terrain (" + (trs.position.x / terrainData.size.x) + ", " + (trs.position.z / terrainData.size.z) + ")";
		AssetDatabase.CreateAsset(terrainData, "Assets/Resources/" + assetName + ".asset");
		if (collider == null)
			collider = terrain.GetComponent<TerrainCollider>();
		width = terrainData.heightmapResolution;
		height = terrainData.heightmapResolution;
		offset = new Vector2(trs.position.x, trs.position.z);
		heights = new float[height, width];
		// updateRoutine = EditorCoroutineUtility.StartCoroutine(UpdateRoutine (), this);
	}

	// public virtual void OnDisable ()
	// {
	// 	EditorCoroutineUtility.StopCoroutine(updateRoutine);
	// }

	// public virtual IEnumerator UpdateRoutine ()
	public virtual void Update ()
	{
		for (int x = 0; x < width; x ++)
		{
			for (int y = 0; y < height; y ++)
			{
				heights[y, x] = Mathf.PerlinNoise(perlinNoiseScale * (x + offset.x) + perlinNoiseOffset.x, perlinNoiseScale * (y + offset.y) + perlinNoiseOffset.y) * hillHeight;
				xNormalized = 0;
				if (x > 0)
					xNormalized = 1f / ((float) width / x);
				heightInfluence = xMultiplyToHeight.Evaluate(xNormalized);
				heights[y, x] *= heightInfluence;
				zNormalized = 0;
				if (y > 0)
					zNormalized = 1f / ((float) height / y);
				heightInfluence = zMultiplyToHeight.Evaluate(zNormalized);
				heights[y, x] *= heightInfluence;
				xNormalized = 0;
				if (x > 0)
					xNormalized = 1f / ((float) width / x);
				heightInfluence = xAddToHeight.Evaluate(xNormalized);
				heights[y, x] += heightInfluence;
				zNormalized = 0;
				if (y > 0)
					zNormalized = 1f / ((float) height / y);
				heightInfluence = zAddToHeight.Evaluate(zNormalized);
				heights[y, x] += heightInfluence;
			}
		}
		if (meld)
			MeldToNeighbors ();
		terrainData.SetHeights(0, 0, heights);
		terrain.terrainData = terrainData;
		collider.terrainData = terrainData;
	}

	public virtual void MeldToNeighbors ()
	{
		int samplesToMeld = 1;
		int x;
		int y;
		int neighborIndex = terrainPositions.IndexOf(terrainPosition + Vector2Int.down);
		if (neighborIndex != -1)
		{
			for (y = 0; y < samplesToMeld; y ++)
			{
				for (x = 0; x < width; x ++)
				{
					heights[y, x] = (terrainGenerators[neighborIndex].terrainData.GetHeight(x, height - 1 - y) / baseTerrainData.size.y + heights[y, x]) / 2;
					terrainGenerators[neighborIndex].heights[height - 1 - y, x] = heights[y, x];
				}
			}
			terrainGenerators[neighborIndex].terrainData.SetHeights(0, 0, terrainGenerators[neighborIndex].heights);
		}
		neighborIndex = terrainPositions.IndexOf(terrainPosition + Vector2Int.up);
		if (neighborIndex != -1)
		{
			for (y = height - 1 - samplesToMeld; y < height; y ++)
			{
				for (x = 0; x < width; x ++)
				{
					heights[y, x] = (terrainGenerators[neighborIndex].terrainData.GetHeight(x, height - 1 - y) / baseTerrainData.size.y + heights[y, x]) / 2;
					terrainGenerators[neighborIndex].heights[height - 1 - y, x] = heights[y, x];
				}
			}
			terrainGenerators[neighborIndex].terrainData.SetHeights(0, 0, terrainGenerators[neighborIndex].heights);
		}
		neighborIndex = terrainPositions.IndexOf(terrainPosition + Vector2Int.left);
		if (neighborIndex != -1)
		{
			for (x = 0; x < samplesToMeld; x ++)
			{
				for (y = 0; y < height; y ++)
				{
					heights[y, x] = (terrainGenerators[neighborIndex].terrainData.GetHeight(width - 1 - x, y) / baseTerrainData.size.y + heights[y, x]) / 2;
					terrainGenerators[neighborIndex].heights[y, width - 1 - x] = heights[y, x];
				}
			}
			terrainGenerators[neighborIndex].terrainData.SetHeights(0, 0, terrainGenerators[neighborIndex].heights);
		}
		neighborIndex = terrainPositions.IndexOf(terrainPosition + Vector2Int.right);
		if (neighborIndex != -1)
		{
			for (x = width - 1 - samplesToMeld; x < width; x ++)
			{
				for (y = 0; y < height; y ++)
				{
					heights[y, x] = (terrainGenerators[neighborIndex].terrainData.GetHeight(width - 1 - x, y) / baseTerrainData.size.y + heights[y, x]) / 2;
					terrainGenerators[neighborIndex].heights[y, width - 1 - x] = heights[y, x];
				}
			}
			terrainGenerators[neighborIndex].terrainData.SetHeights(0, 0, terrainGenerators[neighborIndex].heights);
		}
	}

	public virtual void OnDestroy ()
	{
		int myIndex = terrainPositions.IndexOf(terrainPosition);
		if (myIndex == -1)
			return;
		terrainGenerators.RemoveAt(myIndex);
		terrainPositions.RemoveAt(myIndex);
	}
}
#endif