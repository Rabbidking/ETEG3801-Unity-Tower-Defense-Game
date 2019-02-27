using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
	public NavMeshAgent nma;
	public Vector3 Startpos, Endpos;
	List<NavMeshAgent> enemies = new List<NavMeshAgent>();
	// Start is called before the first frame update
	// Use this for initialization
	void Start()
	{
		Startpos.y += 2f;
		Endpos.y += 2f;
		nma.transform.position = Startpos;
		InvokeRepeating("SpawnNext", 5.0f, 5.0f);
	}

	void SpawnNext()
	{
		enemies.Add(Instantiate(nma, Startpos, Quaternion.identity));
	}


	// Update is called once per frame
	void Update()
    {
		foreach(NavMeshAgent nmas in enemies)
		{
			nmas.SetDestination(Endpos);
		}
    }
}
