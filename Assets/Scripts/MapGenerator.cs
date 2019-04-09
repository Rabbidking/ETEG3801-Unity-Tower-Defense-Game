using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
	public int size;

	public float height;
	public int gridSize;
	public int maxRun;
	public float gridScale;

	public Texture2D buildable;
	public Texture2D path;

	public Terrain current;
	public BuildMap buildMap;
    public Vector3 StartPos;

	public Spawner Spawner;
    public EndPoint endpointPrefab;

    public List<EndPoint> ends;

	private NavMeshDataInstance currentNavMesh;

    public static MapGenerator instance;

	public void Start() {
        instance = this;
		Generate();
	}

	public void Generate()
	{
		buildMap.SetMapValues(size, gridSize, height);

		TerrainData data = new TerrainData();
		//due to Terrian Magic =( the heightmapResolution is always 2**n + 1 for some n
		//so we get a resolution as close to our input size as possible but the world unit size is the same.

		data.heightmapResolution = size;
		data.size = new Vector3(size, height, size);

		size = data.heightmapResolution;
		data.alphamapResolution = size;


        int x = Random.Range(1, gridSize - 2);
		int y = -1;
		int steps = gridSize;
        int length = 0;
		StartPos = new Vector3(0.5f, 0, (x + 0.5f));
		Vector2Int[] dirs = new Vector2Int[] { Vector2Int.right, Vector2Int.left };
		Vector2Int dir = Vector2Int.down;

		while (y < gridSize - 1)
		{
			steps = Random.Range(2, maxRun);
			while (steps > 0)
			{
				if (x + dir.x < 1 || x + dir.x > gridSize - 2 || y - dir.y > gridSize - 1)
				{
					break;
				}

				x += dir.x;
				y -= dir.y;
				buildMap.Tiles[x, y] = TILE_TYPE.PATH;
                length++;
				steps--;
			}

			if (dir == Vector2Int.down)
			{
                if (x == 1)
                {
                    dir = Vector2Int.right;
                }
                else if (x == gridSize - 2)
                {
                    dir = Vector2Int.left;
                }
                else
                {
                    dir = dirs[Random.Range(0, dirs.Length)];
                }
			}
			else
			{
				dir = Vector2Int.down;
			}
		}
		gridScale = ((float)data.size.x) / gridSize;
        StartPos *= gridScale;
		float[,] heights = new float[size, size];
		float scale = size / (float)gridSize;
		for (x = 0; x < gridSize; x++)
		{
			for (y = 0; y < gridSize; y++)
			{
				if (buildMap.Tiles[x, y] != TILE_TYPE.PATH)
				{
                    // if tile is buildable //
                    buildMap.Tiles[x, y] = TILE_TYPE.BUILDABLE;
                    continue;
				}

				int sx = Mathf.RoundToInt(x * scale);
				int sy = Mathf.RoundToInt(y * scale);
				int ex = Mathf.RoundToInt((x + 1) * scale);
				int ey = Mathf.RoundToInt((y + 1) * scale);

				for (int cx = sx; cx < ex; cx++)
				{
					for (int cy = sy; cy < ey; cy++)
					{
						heights[cx, cy] = 1;
					}
				}
			}
		}
		float[,,] textures = new float[size, size, 2];
		for (x = 0; x < size; x++)
		{
			for (y = 0; y < size; y++)
			{
				if (heights[x, y] == 0)
				{
					heights[x, y] = 1;
					textures[x, y, 0] = 0;
					textures[x, y, 1] = 1;
				}
				else
				{
					heights[x, y] = 0;
					textures[x, y, 0] = 1;
					textures[x, y, 1] = 0;
				}
			}
		}

		TerrainLayer pathLayer = new TerrainLayer();
		pathLayer.diffuseTexture = path;

		TerrainLayer buildableLayer = new TerrainLayer();
		buildableLayer.diffuseTexture = buildable;
		data.terrainLayers = new TerrainLayer[] { pathLayer, buildableLayer };
		data.SetHeights(0, 0, heights);
		data.SetAlphamaps(0, 0, textures);

		buildMap.MapTerrain = Terrain.CreateTerrainGameObject(data);
		current = buildMap.MapTerrain.GetComponent<Terrain>();
		buildMap.MapTerrain.transform.SetParent(buildMap.transform, false);
		Vector3 offset = new Vector3(data.size.x / -2f, 0, data.size.z / -2f);
		buildMap.MapTerrain.transform.localPosition += offset;
		buildMap.MapTerrain.layer = buildMap.MapLayer;
		buildMap.FinishSetup();

		List<NavMeshBuildSource> sourceList = new List<NavMeshBuildSource>();
		List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
		NavMeshBuilder.CollectSources(new Bounds(Vector3.zero, Vector3.one * 1000), 0 | (1 << 9), NavMeshCollectGeometry.RenderMeshes, 0, markups, sourceList);
		NavMeshBuildSettings nmbs = NavMesh.CreateSettings();
		nmbs.agentTypeID = Spawner.nma[0].agentTypeID;
		NavMeshData nmd = NavMeshBuilder.BuildNavMeshData(nmbs, sourceList, new Bounds(transform.position, Vector3.one * 1000), Vector3.zero, Quaternion.identity);

		currentNavMesh = NavMesh.AddNavMeshData(nmd);
		Spawner spawn = Instantiate(Spawner, StartPos, Quaternion.identity);
		Spawner.Startpos = StartPos;

        EndPoint ep = GenerateEndPoint(endPos);
        endPoint.transform.localScale = new Vector3(gridScale, 2, gridScale);
	}

	public void CleanUp() {
		NavMesh.RemoveNavMeshData(currentNavMesh);
	}

    public EndPoint GenerateEndPoint(Vector3 point) {
        EndPoint ep = GameObject.Instantiate<EndPoint>(endpointPrefab);
        Vector3 endPos = new Vector3(y + 0.5f, 0, x + 0.5f) * gridScale;
        Vector3 offset = new Vector3(data.size.x / -2f, 0, data.size.z / -2f);
        endPos = endPos + Vector3.up;
        return ep;
    }
}


public static class ListExt {
    public static T Random<T>(this List<T> l)
    {
        if (l.Count == 0)
        {
            return default(T);
        }
        return l[UnityEngine.Random.Range(0,l.Count)];
    }
}