using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
	public int size;

	public float height;
	public int gridSize;
	public int maxRun;

	public Texture2D buildable;
	public Texture2D path;

	public Terrain current;
	public BuildMap buildMap;
	public Spawner Spawner;
    public EndPoint endpointPrefab;

    public float scale;
    public float gridScale;

    public List<EndPoint> ends;

	private List<NavMeshDataInstance> currentNavMeshs;

    public static MapGenerator instance;

	public void Start() {
        instance = this;
        currentNavMeshs = new List<NavMeshDataInstance>();
        GenerateStart();
    }


    //side is the side to start generation, 0 = right, 1 = down, 2 = left, 3 = up 
    private Vector2Int[] dirs = new Vector2Int[] { Vector2Int.right,Vector2Int.down, Vector2Int.left, Vector2Int.up};

    public void GenerateStart()
	{
        buildMap.SetMapValues(size, gridSize, height);

		TerrainData data = new TerrainData();
        TILE_TYPE[,] tiles = new TILE_TYPE[gridSize,gridSize];
		//due to Terrian Magic =( the heightmapResolution is always 2**n + 1 for some n
		//so we get a resolution as close to our input size as possible but the world unit size is the same.

		data.heightmapResolution = size;
		data.size = new Vector3(size, height, size);

		int rsize = data.heightmapResolution;
		data.alphamapResolution = rsize;

        Vector2Int StartPos =  new Vector2Int(UnityEngine.Random.Range(1, gridSize - 2),-1);
        Vector2Int current = StartPos;

        int steps;
        Vector2Int[] dirs = new Vector2Int[] { Vector2Int.left, Vector2Int.right };
        Vector2Int dir = Vector2Int.up;
        List<Vector2Int> moves = new List<Vector2Int>();

        while (current.y < gridSize - 1)
		{
			steps = UnityEngine.Random.Range(2, maxRun);
			while (steps > 0)
			{
               
				if (current.x + dir.x < 1 || current.x + dir.x > gridSize - 2 || current.y - dir.y >= gridSize - 2)
				{
					break;
				}
                current += dir;
                moves.Add(dir);
                tiles[current.x, current.y] = TILE_TYPE.PATH;
				steps--;
			}
			if (dir == Vector2Int.up)
			{
                if (current.x == 1)
                {
                    dir = Vector2Int.right;
                }
                else if (current.x == gridSize - 2)
                {
                    dir = Vector2Int.left;
                }
                else
                { 
                    dir = dirs.Random();
                }
			}
			else
			{
				dir = Vector2Int.up;
			}
		}

        //find center
        Vector2 center = StartPos;
        for (int i = 0; i < moves.Count / 2; i++) {
            center += moves[i];
        }

		float[,] heights = new float[rsize, rsize];
		scale = rsize / (float)gridSize;
        gridScale = ((float)size) / gridSize;
		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{
				if (tiles[x, y] != TILE_TYPE.PATH)
				{
                    // if tile is buildable //
                    tiles[x, y] = TILE_TYPE.BUILDABLE;
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

        //assign Textures and flip heights
		float[,,] textures = new float[rsize,rsize, 2];
		for (int x = 0; x < rsize; x++)
		{
			for (int y = 0; y < rsize; y++)
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

        //Set Terrian Textures
		TerrainLayer pathLayer = new TerrainLayer();
        pathLayer.diffuseTexture = path;
		TerrainLayer buildableLayer = new TerrainLayer();
		buildableLayer.diffuseTexture = buildable;
        data.terrainLayers = new TerrainLayer[] { pathLayer, buildableLayer };
        data.SetAlphamaps(0, 0, textures);

        //Set Terrian Heights
		data.SetHeights(0, 0, heights);

        //Build map stuff
		GameObject MapTerrain = Terrain.CreateTerrainGameObject(data);
		this.current = MapTerrain.GetComponent<Terrain>();
		MapTerrain.transform.SetParent(buildMap.transform, false);
		Vector3 offset = new Vector3(size / -2f, 0,size / -2f);
		MapTerrain.transform.localPosition += offset;
        MapSection ms = MapTerrain.AddComponent<MapSection>();
        ms.tiles = tiles;
        ms.Section = Vector2Int.zero;
        MapTerrain.layer = buildMap.MapLayer;
		buildMap.FinishSetup();

        //Generate Navmesh stuff
        GeanerateNavMeshData();

        //SetSpawn
        Vector3 SpawnPoint = new Vector3(center.y +0.5f, 0, center.x + 0.5f) *gridScale + offset;
        Spawner.transform.position = SpawnPoint;
        Spawner.Startpos = SpawnPoint;

        //Generate EndPoints
        GenerateEndPoint(StartPos,1, Vector2Int.zero);
        GenerateEndPoint(current+ Vector2Int.up,3, Vector2Int.zero);
    }
    public void Generate() {
        Generate(ends.Random());
    }
    public void Generate(EndPoint end)
    {
        //buildMap.SetMapValues(size, gridSize, height);

        TerrainData data = new TerrainData();
        //due to Terrian Magic =( the heightmapResolution is always 2**n + 1 for some n
        //so we get a resolution as close to our input size as possible but the world unit size is the same.

        TILE_TYPE[,] tiles = new TILE_TYPE[gridSize, gridSize];
        data.heightmapResolution = size;
        data.size = new Vector3(size, height, size);

        int rsize = data.heightmapResolution;
        data.alphamapResolution = rsize;
        Vector2Int current = Vector2Int.zero;
        Func<Vector2Int, bool> isFinished = (c) => { return true; } ;
        Func<Vector2Int, Vector2Int, bool> inLimits = (c,d) => { return
            c.x + d.x < 1 || 
            c.x + d.x > gridSize - 2 ||
            c.y - d.y >= gridSize - 2 ||
            c.y - d.y > 1;
        };
        //change this to mathy solution
        //side is the side to start generation, 0 = right, 1 = down, 2 = left, 3 = up 
        switch (end.MapDirection) {
            case 0:
                //right
                current = new Vector2Int(gridSize, end.gridPoint.y);
                break;
            case 1:
                //down
                isFinished = (c) => {return c.y <= 0; };
                inLimits = (c, d) => {
                    return c.x + d.x < 1 || c.x + d.x > gridSize - 2 || c.y - d.y < 1;
                };
                current = new Vector2Int(end.gridPoint.x, gridSize);
                break;
            case 2:
                //left
                current = new Vector2Int(-1,0);
                break;
            case 3:
                //up
                current = new Vector2Int(end.gridPoint.x, -1);
                inLimits = (c, d) => {
                    return c.x + d.x < 1 || c.x + d.x > gridSize - 2 || c.y - d.y >= gridSize - 2 ;
                };
                isFinished = (c) => { return c.y >= gridSize - 1; };
                break;
        }
        Vector2Int Start = current; 
        int steps;
        Vector2Int[] dirs = new Vector2Int[] {this.dirs[(end.MapDirection -1)%4], this.dirs[(end.MapDirection - 1) % 4]};
        Vector2Int dir = this.dirs[end.MapDirection];
        int failSafe = 0;
        while (!isFinished(current) && failSafe < 100)
        {
            steps = UnityEngine.Random.Range(2, maxRun);
            while (steps > 0 && failSafe < 100)
            {
                failSafe++;
                if (inLimits(current, dir) || isFinished(current))
                {
                    print("hsah");
                    break;
                }
                current += dir;
                if (end.MapDirection != 3)
                {
                    print(current);
                }
                tiles[current.x, current.y] = TILE_TYPE.PATH;
                steps--;
            }
            if (dir == this.dirs[end.MapDirection])
            {
                if (current.x == 1)
                {
                    dir = Vector2Int.right;
                }
                else if (current.x == gridSize - 2)
                {
                    dir = Vector2Int.left;
                }
                else
                {
                    dir = dirs.Random();
                }
            }
            else
            {
                dir = this.dirs[end.MapDirection];
            }
        }
        

        float[,] heights = new float[rsize, rsize];
        scale = rsize / (float)gridSize;
        gridScale = size / (float)gridSize;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if(tiles[x, y] != TILE_TYPE.PATH)
                {
                    // if tile is buildable //
                    tiles[x, y] = TILE_TYPE.BUILDABLE;
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

        //assign Textures and flip heights
        float[,,] textures = new float[rsize, rsize, 2];
        for (int x = 0; x < rsize; x++)
        {
            for (int y = 0; y < rsize; y++)
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

        //Set Terrian Textures
        TerrainLayer pathLayer = new TerrainLayer();
        pathLayer.diffuseTexture = path;
        TerrainLayer buildableLayer = new TerrainLayer();
        buildableLayer.diffuseTexture = buildable;
        data.terrainLayers = new TerrainLayer[] { pathLayer, buildableLayer };
        data.SetAlphamaps(0, 0, textures);

        //Set Terrian Heights
        data.SetHeights(0, 0, heights);

        GameObject MapTerrain = Terrain.CreateTerrainGameObject(data);
        this.current = MapTerrain.GetComponent<Terrain>();
        MapTerrain.transform.SetParent(buildMap.transform, false);
        Vector3 offset = new Vector3(-0.5f + end.terrian.y + this.dirs[end.MapDirection].y, 0, -0.5f+end.terrian.x + this.dirs[end.MapDirection].x) * gridScale*gridSize;
        MapTerrain.transform.localPosition += offset;
        MapSection ms = MapTerrain.AddComponent<MapSection>();
        ms.tiles = tiles;
        ms.Section = end.terrian + this.dirs[end.MapDirection];
        MapTerrain.layer = buildMap.MapLayer;

        buildMap.FinishSetup();

        //Generate Navmesh stuff
        GeanerateNavMeshData();
        NavMeshLinkData nmld = new NavMeshLinkData();
        nmld.agentTypeID = Spawner.nma[0].agentTypeID;
        nmld.area = 0;
        nmld.bidirectional = true;
        nmld.width = gridScale;
        nmld.startPosition = new Vector3(Start.y + 0.5f, 0, Start.x + 0.5f) * gridScale + offset;
        Start += this.dirs[end.MapDirection];
        nmld.endPosition = new Vector3(Start.y + 0.5f, 0, Start.x + 0.5f) * gridScale + offset;
        NavMesh.AddLink(nmld);

        //move end point
        Vector3 endPos = new Vector3(current.y + 0.5f, 0, current.x + 0.5f) * gridScale + offset;
        end.transform.position = endPos;
        end.transform.localScale = Vector3.one * gridScale;
        end.gridPoint = current;
        end.terrian = ms.Section;
    }

    public void GeanerateNavMeshData()
    { 
        List<NavMeshBuildSource> sourceList = new List<NavMeshBuildSource>();
        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
        NavMeshBuilder.CollectSources(new Bounds(Vector3.zero, Vector3.one * 1000), 0 | (1 << 9), NavMeshCollectGeometry.RenderMeshes, 0, markups, sourceList);
        NavMeshBuildSettings nmbs = NavMesh.CreateSettings();
        nmbs.agentTypeID = Spawner.nma[0].agentTypeID;
        NavMeshData nmd = NavMeshBuilder.BuildNavMeshData(nmbs, sourceList, new Bounds(transform.position, Vector3.one * 1000), Vector3.zero, Quaternion.identity);
        currentNavMeshs.Add(NavMesh.AddNavMeshData(nmd));

    }

    public void CleanUp() {
        foreach(NavMeshDataInstance cnm in currentNavMeshs)
		    NavMesh.RemoveNavMeshData(cnm);
	}

    public void GenerateEndPoint(Vector2Int point, int Direction, Vector2Int Terrain) {
        EndPoint ep = GameObject.Instantiate<EndPoint>(endpointPrefab);
        Vector3 offset = new Vector3((gridScale * gridSize) / -2f, 0, (gridScale * gridSize) / -2f);
        offset -= new Vector3(dirs[Direction].y, 0, dirs[Direction].x) * (gridScale/10);
        Vector3 endPos = new Vector3(point.y + 0.5f, 0, point.x + 0.5f) * gridScale  + offset;
        ep.transform.position = endPos;
        ep.transform.localScale = Vector3.one * gridScale;

        ep.MapDirection = Direction;
        ep.gridPoint = point;
        ends.Add(ep);
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


    public static T Random<T>(this T[] l)
    {
        if (l.Length == 0)
        {
            return default(T);
        }
        return l[UnityEngine.Random.Range(0, l.Length)];
    }
}