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
        //Commited out for  2/28/2019 demo
        //enemies.Add(Instantiate(nma, Startpos, Quaternion.identity));

        NavMeshAgent obj = Instantiate(nma, Startpos, Quaternion.identity);
        enemies.Add(obj);
        obj.SetDestination(Endpos);
    }


    // Update is called once per frame
    void Update()
    {
        /*Commited out for  2/28/2019 demo
		foreach(NavMeshAgent nmas in enemies)
		{
			nmas.SetDestination(Endpos);
		}
        */
    }
}
