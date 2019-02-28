using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGen : MonoBehaviour
{
	public Spawner Spawner;
    public GameObject endPoint;
	MapGenerator mg;
	// Start is called before the first frame update
	void Start()
    {
		mg = GetComponent<MapGenerator>();
		mg.Generate();
		List<NavMeshBuildSource> sourceList = new List<NavMeshBuildSource>();
		List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
		NavMeshBuilder.CollectSources(new Bounds(Vector3.zero, Vector3.one * 1000), 0 | (1 << 9), NavMeshCollectGeometry.RenderMeshes, 0 , markups, sourceList);
		NavMeshBuildSettings nmbs = NavMesh.CreateSettings();
		nmbs.agentTypeID = Spawner.nma.agentTypeID;
		NavMeshData nmd = NavMeshBuilder.BuildNavMeshData(nmbs,sourceList, new Bounds(transform.position, Vector3.one * 1000), Vector3.zero, Quaternion.identity);
		if (!nmd) { print("OH NO"); return; }
		NavMesh.AddNavMeshData(nmd);
		Spawner spawn = Instantiate(Spawner, mg.Start, Quaternion.identity);
		spawn.Startpos = mg.Start;
		spawn.Endpos = mg.End;

        endPoint.transform.position = mg.End + Vector3.up;
        endPoint.transform.localScale = new Vector3(mg.gridScale,2,mg.gridScale);
	}

	// Update is called once per frame
	void Update()
    {

		
	}
}
