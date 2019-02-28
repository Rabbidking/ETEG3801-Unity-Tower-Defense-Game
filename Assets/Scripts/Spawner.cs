using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
	public NavMeshAgent nma;
	public Vector3 Startpos, Endpos;
	List<NavMeshAgent> enemies = new List<NavMeshAgent>();
	float stime, etime = 4.0f;
	int size;
	// Start is called before the first frame update
	// Use this for initialization
	void Start()
	{
		Startpos.y += 2f;
		Endpos.y += 2f;
		nma.transform.position = Startpos;
		size = 0;
		//InvokeRepeating("SpawnNext", 5.0f, 5.0f);
	}

	void SpawnNext()
	{
		//Commited out for  2/28/2019 demo
		//enemies.Add(Instantiate(nma, Startpos, Quaternion.identity));
		//int size = Random.Range(1, 5);
		//StartCoroutine(SpawnWave(size));
	}

	IEnumerator SpawnWave(int size) {
		for (int i = 0; i < size; i++)
		{
			int gsize = Random.Range(1, 3);
			for (int j = 0; j < gsize; j++)
			{
				NavMeshAgent obj = Instantiate(nma, Startpos, Quaternion.identity);
				enemies.Add(obj);
				obj.SetDestination(Endpos);
			}
			yield return new WaitForSeconds(1);
		}
	}

    // Update is called once per frame
    void Update()
    {
		stime += Time.deltaTime;
		
		if (stime >= etime + size)
		{
			size = Random.Range(1, 5);
			stime = 0;
			etime = Random.Range(1.0f, 5.0f);
			StartCoroutine(SpawnWave(size));
		}
	}
}
