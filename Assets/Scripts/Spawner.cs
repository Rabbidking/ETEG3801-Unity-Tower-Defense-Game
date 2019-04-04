using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
	struct waveinfo
	{
		public int unitNum;
		public int numToSpawn;
	}

	public ResourceManager WaveMaster;
	public List<NavMeshAgent> nma;
	public Vector3 Startpos, Endpos;
	List<NavMeshAgent> enemies = new List<NavMeshAgent>();
	float stime, etime = 4.0f;
	List<waveinfo> Wave = new List<waveinfo>();
	int waveVal;
	bool isWave;


	void Start()
	{
		isWave = false;
		waveVal = 5;
		Startpos.y += 2f;
		Endpos.y += 2f;
		foreach(NavMeshAgent blah in nma)
			blah.transform.position = Startpos;
	}
	

	IEnumerator SpawnWave() {
		WaveMaster.CurWave++;
		WaveMaster.UpdateWaveText();
		for (int i = 0; i < waveVal; i++)
		{
			waveinfo wi;
			wi.unitNum = Random.Range(0, 3);
			if (wi.unitNum == 1)
			{
				wi.numToSpawn = 1;
			}else{
				wi.numToSpawn = Random.Range(1, 3);
			}
			Wave.Add(wi);
		}
		if(WaveMaster.CurWave %5 == 0)
		{
			waveinfo wi;
			wi.unitNum = Random.Range(4, nma.Count-1);
			//wi.unitNum = nma.Count - 1;
			wi.numToSpawn = 1;
			Wave.Add(wi);
		}
		waveVal += Random.Range(0, 4);
		for (int i = 0; i < Wave.Count; i++)
		{
			for (int j = 0; j < Wave[i].numToSpawn; j++)
			{
				NavMeshAgent obj = Instantiate(nma[Wave[i].unitNum], Startpos, Quaternion.identity);
				obj.tag = "Enemy";
				enemies.Add(obj);
				obj.SetDestination(Endpos);
			}
			yield return new WaitForSeconds(1);
		}
		Wave = new List<waveinfo>();
		
		isWave = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (!isWave)
		{
			stime += Time.deltaTime;

			if (stime >= etime)
			{
				isWave = true;
				stime = 0;
				etime = 30;
				StartCoroutine(SpawnWave());
			}
		}
	}
}